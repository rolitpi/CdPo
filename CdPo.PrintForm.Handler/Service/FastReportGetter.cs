using System.Data;

using CdPo.Common.Enum;
using CdPo.PrintForm.Handler.Interfaces;

using FastReport;
using FastReport.Export;
using FastReport.Export.OoXML;
using FastReport.Export.Pdf;

using Microsoft.Extensions.Logging;

namespace CdPo.PrintForm.Handler.Service;

/// <summary>
/// Заполнение отчета средствами FastReport
/// </summary>
public class FastReportGetter : IReportGetter
{
    protected bool Disposed { get; set; }
    protected IDictionary<string, object> LocalParameters { get; set; }
    protected ExportBase Exporter { get; }
    protected Report Report { get; }

    protected static readonly string[] AdditionalReferencedAssemblies = {
        "System.Collections.NonGeneric",
        "System.Collections",
        "System.Drawing.Primitives",
        "System.Drawing.Common",
        "System.Data.Common",
        "System.ComponentModel.TypeConverter"
    };
    
    private readonly ILogger _logger;

    /// <remarks>
    /// Значение устанавливается только после вызова метода _report.Prepare.
    /// </remarks>
    public int? ReportPageCount => Report?.PreparedPages?.Count;
    
    public FastReportGetter(int printFileExt, ILogger logger)
    {
        _logger = logger;
        Exporter = printFileExt switch
        {
            (int)PrintFileExt.Pdf => new PDFExport
            {
                ShowProgress = false, ImagesOriginalResolution = true, EmbeddingFonts = true
            },
            (int)PrintFileExt.Word => new Word2007Export
            {
                Wysiwyg = true, RowHeight = Word2007Export.RowHeightType.Exactly
            },
            (int)PrintFileExt.Excel => new Excel2007Export { Wysiwyg = false },
            _ => new PDFExport { ShowProgress = false, ImagesOriginalResolution = true, EmbeddingFonts = true }
        };

        Report = new Report();
    }

    public Task StoreLocalPropertiesAsync(IDictionary<string, object> properties, CancellationToken cancellationToken = default)
    {
        LocalParameters = properties ?? new Dictionary<string, object>();
        return Task.CompletedTask;
    }

    public virtual Task<Stream> GetReportAsync(Stream template, DataSet model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        _logger.LogDebug("BaseFastReportGetter.GetReport");
        Report.Load(template);

        Report.RegisterData(model, "Data");
        foreach (var property in LocalParameters)
        {
            Report.SetParameterValue(property.Key, property.Value);
        }

        SetExtendedReferenceList(Report);
        _logger.LogDebug("BaseFastReportGetter.Prepare");
        Report.Prepare();

        var stream = new MemoryStream();

        _logger.LogDebug("BaseFastReportGetter.Export");
        Exporter.Export(Report, stream);

        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);

        _logger.LogDebug("BaseFastReportGetter.End");
        return Task.FromResult<Stream>(stream);
    }

    public virtual void Dispose()
    {
        _logger.LogDebug("BaseFastReportGetter.Dispose");
        Clear();
        Disposed = true;
        Exporter.Dispose();
        Report.Dispose();
        GC.Collect();
    }

    /// <summary>
    /// Расширить список ссылок для отчета
    /// </summary>
    /// <param name="report"></param>
    protected static void SetExtendedReferenceList(Report report)
    {
        var oldAssemblies = report.ReferencedAssemblies.ToList();
        oldAssemblies.AddRange(AdditionalReferencedAssemblies);
        report.ReferencedAssemblies = oldAssemblies.ToArray();
    }

    /// <summary>
    /// Очистить ресурсы шаблона
    /// </summary>
    private void Clear()
    {
        if (Disposed)
        {
            throw new ObjectDisposedException(nameof(FastReportGetter));
        }

        Exporter.Clear();
        Report.Clear();
    }
}