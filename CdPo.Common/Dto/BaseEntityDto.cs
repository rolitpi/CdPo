using CdPo.Common.Entity;

namespace CdPo.Common.Dto;

/// <summary>
/// Базовое дто сущности бд
/// </summary>
public class BaseEntityDto: IHaveId
{
    /// <inheritdoc/>
    public long Id { get; set; }
}