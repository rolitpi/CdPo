using CdPo.Common.Dto;
using CdPo.Common.Entity;
using CdPo.Common.Enum;
using CdPo.Common.Extensions;
using CdPo.Model.Dto;
using CdPo.Model.Interfaces.Files;
using CdPo.Model.Interfaces.PrintForms;

using Microsoft.AspNetCore.Mvc;

namespace CdPo.Web.Controllers;

/// <summary>
/// Контроллер для работы с печатными формами
/// </summary>
[Route("api/[controller]")]
public class PrintFormsController: BaseController
{
    private readonly IPrintFileService _printFileService;
    private readonly IFileManager _fileManager;
    
    /// <inheritdoc cref="BaseController{TEntity}"/>
    public PrintFormsController(
        ILogger<BaseController> logger,
        IPrintFileService printFileService,
        IFileManager fileManager) : base(logger)
    {
        _printFileService = printFileService;
        _fileManager = fileManager;
    }
    
    /// <summary>
    /// Получить список всех доступных печатных форм
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var printForms = Enum.GetValues(typeof(PrintFileType)).Cast<PrintFileType>()
            .Select(x => new EntityReferenceWithNameDto<BaseEntity>
            {
                Id = (int)x,
                Name = x.GetDisplayName()
            });
        return await Task.FromResult(Ok(printForms));
    }

    /// <summary>
    /// Получить сформированную конкретную печатную форму
    /// </summary>
    /// <param name="id">Идентификатор печатной формы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        //todo: убрать хардкод расширения
        const string mimeType = "application/pdf";
        var printFormParams = new PrintFormParams
        {
            EntityId = 1,
            PrintFormType = id,
        };

        try
        {
            var fileMetadata = await _printFileService.CreatePrintFileAsync(printFormParams, cancellationToken);
            if (fileMetadata == default)
            {
                return NotFound("Не удалось получить печатную форму с такими параметрами");
            }

            var stream = await _fileManager.GetFileAsync(fileMetadata, cancellationToken);
            var fileName = ((PrintFileType)id).GetDisplayName();
            return File(stream, mimeType, $"{fileName}.pdf");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Произошла ошибка при попытке формирования печатной формы");
            return BadRequest();
        }
    }
}