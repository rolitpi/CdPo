using Castle.Windsor;

using CdPo.Model.Dto;
using CdPo.Model.Entities;
using CdPo.Model.Interfaces.Files;
using CdPo.Model.Interfaces.PrintForms;
using CdPo.PrintForm.Handler.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CdPo.PrintForm.Handler.Service;

/// <inheritdoc cref="IPrintFileService"/>
public class PrintFileService: IPrintFileService
{
    private readonly ILogger<PrintFileService> _logger;
    private readonly IWindsorContainer _container;

    public PrintFileService(ILogger<PrintFileService> logger, IWindsorContainer container)
    {
        _logger = logger;
        _container = container;
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
        
        try
        {
            return await Task.FromResult(new FileMetadata());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Произошла ошибка при формировании печатной формы {printFormParams.PrintFormType}");
            throw;
        }
    }
    
    /// <summary>
    /// Получить механизм сбора данных для отчёта
    /// </summary>
    private AbstractReportQuery GetPrintFormQuery(string reportName)
    {
        return _container.Resolve<AbstractReportQuery>(reportName);
    }
}