using CdPo.Model.Dto;
using CdPo.Model.Interfaces.Files;

namespace CdPo.Model.Interfaces.PrintForms;

/// <summary>
/// Интерфейс для формирования печатной формы
/// </summary>
public interface IPrintFileService
{
    /// <summary>
    /// Сформировать печатную форму
    /// </summary>
    /// <param name="printFormParams">Параметры для формирования печатной формы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<IFileMetadata> CreatePrintFileAsync(PrintFormParams printFormParams,
        CancellationToken cancellationToken = default);
}