using CdPo.Common.Entity;
using CdPo.Model.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CdPo.Web.Controllers;

/// <summary>
/// Api-контроллер сущности
/// </summary>
/// <typeparam name="TEntity">Тип сущности - наследника <see cref="BaseEntity"/></typeparam>
public class BaseController<TEntity>: BaseController
    where TEntity: BaseEntity, new()
{
    /// <summary>
    /// Репозиторий для работы с сущностями <see cref="TEntity"/>
    /// </summary>
    protected readonly IGenericRepository<TEntity> Repository;
    
    public BaseController(ILogger<BaseController> logger, IGenericRepository<TEntity> repository) 
        : base(logger)
    {
        Repository = repository;
    }
    
    /// <summary>
    /// Получить объект сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetAsync(id, cancellationToken);
        return Ok(entity);
    }
    
    /// <summary>
    /// Получить все объекты сущностей по идентификатору
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.GetAllAsync(cancellationToken: cancellationToken);
        return Ok(entities);
    }

    /// <summary>
    /// Изменить существующий объект
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost("{id:long}")]
    public async Task<IActionResult> Post(long id, [FromBody] TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.Id = id;
        var result = await Repository.UpdateAsync(entity, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Добавить новый объект
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await Repository.AddAsync(entity, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Удалить существующий объект
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpDelete("{id:long}")]
    public async Task Delete(long id, CancellationToken cancellationToken = default)
    {
        await Repository.DeleteAsync(new TEntity {Id = id}, cancellationToken);
    }
}

/// <summary>
/// Базовый api-контроллер
/// </summary>
public class BaseController : Controller
{
    protected readonly ILogger<BaseController> Logger;
    public BaseController(ILogger<BaseController> logger)
    {
        Logger = logger;
    }
}