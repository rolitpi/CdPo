using CdPo.Common.Entity;
using CdPo.Model.Attributes;

namespace CdPo.Model.Entities;

/// <summary>
/// Обучающийся
/// </summary>
[GeneratedController("student", "Обучающиеся")]
public class Student: BaseEntity
{
    /// <summary>
    /// Общая информация о человеке
    /// </summary>
    public virtual Person Person { get; set; }
    
    /// <summary>
    /// Группа, в которой состоит обучающийся
    /// </summary>
    public virtual Group Group { get; set; }
}