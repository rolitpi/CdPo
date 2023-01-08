using CdPo.Common.Entity;

namespace CdPo.Common.Dto;

/// <summary>
/// Дто-обертка сущности из бд с названием
/// </summary>
public class EntityReferenceWithNameDto<TEntity>: EntityReferenceDto<TEntity>
    where TEntity: BaseEntity, new()
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
}