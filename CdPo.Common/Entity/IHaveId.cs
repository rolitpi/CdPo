namespace CdPo.Common.Entity;

/// <summary>Интерфейс объекта, имеющего идентификатор.</summary>
public interface IHaveId
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    long Id { get; }
}