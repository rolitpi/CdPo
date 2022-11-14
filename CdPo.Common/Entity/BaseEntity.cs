using System.Text.Json.Serialization;

namespace CdPo.Common.Entity;

/// <summary>Базовая сущность</summary>
public class BaseEntity : PersistentObject
{
    /// <summary>Дата создания</summary>
    [JsonIgnore]
    public virtual DateTime ObjectCreateDate { get; set; }

    /// <summary>Дата последнего редактирования</summary>
    [JsonIgnore]
    public virtual DateTime ObjectEditDate { get; set; }

    /// <summary>Версия объекта</summary>
    [JsonIgnore]
    public virtual int ObjectVersion { get; set; }
}