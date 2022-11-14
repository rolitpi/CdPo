namespace CdPo.Common.Entity;

/// <summary>Базовая сущность с идентификатором(Тип идентификатора Int64)</summary>
public class PersistentObject : IEntity, IHaveId
{
    object IEntity.Id
    {
        get => Id;
        set => Id = (long) value;
    }

    /// <summary>Идентификатор</summary>
    public virtual long Id { get; set; }
}