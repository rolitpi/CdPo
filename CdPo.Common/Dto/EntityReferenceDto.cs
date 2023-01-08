using CdPo.Common.Entity;

namespace CdPo.Common.Dto;

/// <summary>
/// Дто-обертка сущности из бд
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EntityReferenceDto<TEntity>: BaseEntityDto
    where TEntity: BaseEntity, new()
{
    /// <summary>
    /// Получить объект сущности из бд
    /// </summary>
    public TEntity GetEntity() => new TEntity { Id = Id };
}