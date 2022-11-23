namespace CdPo.Common.Entity;

/// <summary>
/// Базовое дто сущности бд
/// </summary>
public class BaseEntityDto: IHaveId
{
    /// <inheritdoc/>
    public long Id { get; set; }
}