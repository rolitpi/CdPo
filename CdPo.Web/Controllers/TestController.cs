using CdPo.Common.Entity;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CdPo.Web.Controllers;

[Route("api/test")]
public class TestController: BaseController
{
    private readonly IDataStore _dataStore;
    private readonly IFileManager _fileManager;
    
    public TestController(ILogger<BaseController> logger, IDataStore dataStore, IFileManager fileManager)
        : base(logger)
    {
        _dataStore = dataStore;
        _fileManager = fileManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var file = await _fileManager.GetFileAsync(10, cancellationToken);
        return Ok(file);
        //var path = "C:\\Users\\tsebr\\Desktop\\test.xml";
        //await using var fs = new FileStream(path, FileMode.Open);
        //var fileMetadata = await _fileManager.SaveFileAsync(fs, "testovyj_file.xml", cancellationToken);
        //return Ok(fileMetadata);
    }
}