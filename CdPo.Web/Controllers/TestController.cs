using CdPo.Common.Dto;
using CdPo.Common.Entity;
using CdPo.Common.Enum;
using CdPo.Common.Extensions;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;
using CdPo.Model.Interfaces.PrintForms;

using Microsoft.AspNetCore.Mvc;

namespace CdPo.Web.Controllers;

[Route("api/[controller]")]
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
        var printForms = Enum.GetValues(typeof(PrintFileType)).Cast<PrintFileType>()
            .Select(x => new EntityReferenceWithNameDto<BaseEntity>
            {
                Id = (int)x,
                Name = x.GetDisplayName()
            });
        return Ok(printForms);
    }
}