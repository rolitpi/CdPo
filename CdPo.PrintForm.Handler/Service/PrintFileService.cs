using System.Globalization;
using System.Text;

using Castle.Windsor;

using CdPo.Common.Enum;
using CdPo.Model.Dto;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;
using CdPo.Model.Interfaces.PrintForms;
using CdPo.PrintForm.Handler.Extensions;
using CdPo.PrintForm.Handler.Interfaces;
using CdPo.PrintForm.Handler.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CdPo.PrintForm.Handler.Service;

/// <inheritdoc cref="IPrintFileService"/>
public class PrintFileService: IPrintFileService
{
    private readonly ILogger<PrintFileService> _logger;
    private readonly IWindsorContainer _container;
    private readonly IDataStore _dataStore;
    private readonly IConfiguration _configuration;
    private readonly IFileManager _fileManager;

    public PrintFileService(
        ILogger<PrintFileService> logger,
        IDataStore dataStore,
        IWindsorContainer container,
        IConfiguration configuration,
        IFileManager fileManager)
    {
        _logger = logger;
        _dataStore = dataStore;
        _container = container;
        _configuration = configuration;
        _fileManager = fileManager;
    }

    public async Task<IFileMetadata> CreatePrintFileAsync(PrintFormParams printFormParams,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (printFormParams == default)
        {
            throw new ArgumentNullException(nameof(printFormParams));
        }

        if (printFormParams.EntityId < 0 || printFormParams.PrintFormType <= 0)
        {
            throw new ApplicationException("Переданы некорректные параметры для создания печатной формы");
        }
        
        var fileName = GetOrCreateFileName(printFormParams);
        SetCulture();
        var reportName = printFormParams.PrintFormType.ToString();
        var reportInfo = GetPrintFormQuery(reportName);
        
        try
        {
            using IReportGetter reportGetter = new FastReportGetter(printFormParams.PrintFileTypeInt, _logger);

            reportInfo.AdditionalParams = printFormParams.OtherParams;
            var staticParameters = await reportInfo.GetReportParameters(_dataStore, printFormParams.EntityId);
            var parametersDictionary = staticParameters.GetParametersDictionary();
            await reportGetter.StoreLocalPropertiesAsync(parametersDictionary, cancellationToken);

            var reportData = await reportInfo.GetReportData(_dataStore, printFormParams.EntityId);

            await using var template = GetTemplate(reportInfo);
            await using var result = await reportGetter.GetReportAsync(template, reportData, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            var file = await _fileManager.SaveFileAsync(result, fileName, cancellationToken);

            return file;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Произошла ошибка при формировании печатной формы {printFormParams.PrintFormType}");
            throw;
        }
        finally
        {
            _container.Release(reportInfo);
        }
    }
    
    /// <summary>
    /// Получить шаблон печатной формы
    /// </summary>
    protected virtual Stream GetTemplate(AbstractReportQuery reportQuery)
    {
        var contentPath = _configuration.GetSection("PrintForm.Handler")?.GetValue<string>("ContentPath");

        var filePath = reportQuery
            .GetTemplatePath()
            .Replace(AbstractReportQuery.ReportRootPath, string.Empty)
            .Replace('.', '\\')
            .Replace("\\frx", ".frx");
        return File.OpenRead(contentPath + filePath);
    }
    
    /// <summary>
    /// Установить региональные настройки
    /// </summary>
    protected virtual void SetCulture()
    {
        var culture = new CultureInfo("ru-RU")
        {
            NumberFormat = { NumberDecimalSeparator = ".", CurrencyDecimalSeparator = "." },
            DateTimeFormat = { ShortDatePattern = "dd.MM.yyyy" }
        };
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;

        // TODO Зачем это? По крайней мере зачем это вызывать каждый раз? CodePagesEncodingProvider.Instance - синглтон
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    
    /// <summary>
    /// Получить механизм сбора данных для отчёта
    /// </summary>
    private AbstractReportQuery GetPrintFormQuery(string reportName)
    {
        return _container.Resolve<AbstractReportQuery>(reportName);
    }

    private string GetOrCreateFileName(PrintFormParams printFormParams)
    {
        printFormParams.OtherParams.TryGetValue("FileName", out var fileNameObject);
        var fileName = fileNameObject as string ?? Guid.NewGuid().ToString();

        var fileExtension = printFormParams.PrintFileTypeInt switch
        {
            (int)PrintFileExt.Word => "doc",
            (int)PrintFileExt.Excel => "xlsx",
            (int)PrintFileExt.Pdf => "pdf",
            _ => throw new ArgumentOutOfRangeException(nameof(printFormParams.PrintFileTypeInt))
        };

        return $"{fileName}.{fileExtension}";
    }
}