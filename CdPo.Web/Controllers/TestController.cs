using CdPo.Common.Entity;
using CdPo.Common.Enum;
using CdPo.Model.Dto;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;
using CdPo.Model.Interfaces.PrintForms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CdPo.Web.Controllers;

[Route("api/test")]
public class TestController: BaseController
{
    private readonly IDataStore _dataStore;
    private readonly IFileManager _fileManager;
    private readonly IPrintFileService _printFileService;
    
    public TestController(
        ILogger<BaseController> logger,
        IDataStore dataStore,
        IFileManager fileManager,
        IPrintFileService printFileService)
        : base(logger)
    {
        _dataStore = dataStore;
        _fileManager = fileManager;
        _printFileService = printFileService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var printFormParams = new PrintFormParams
        {
            EntityId = 1,
            PrintFormType = (int)PrintFileType.StatementPpp
        };
        var file = await _printFileService.CreatePrintFileAsync(printFormParams, cancellationToken);
        return Ok();
    }
}