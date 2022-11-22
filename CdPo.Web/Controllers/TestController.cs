using CdPo.Model.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CdPo.Web.Controllers;

/// <summary>
/// Тестовый контроллер
/// </summary>
[Route("[controller]")]
[ApiController]
public class TestController: Controller
{
    private readonly IDataContext _dataContext;
    
    public TestController(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    /// <summary>
    /// Получить персону по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var item = await _dataContext.Persons.FindAsync(id);
        return Ok(item);
    }
}