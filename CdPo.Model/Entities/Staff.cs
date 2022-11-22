using CdPo.Common.Entity;
using CdPo.Model.Attributes;

namespace CdPo.Model.Entities;

/// <summary>
/// Сотрудник
/// </summary>
[GeneratedController("staff", "Сотрудники")]
public class Staff: BaseEntity
{
    /// <summary>
    /// Должность
    /// </summary>
    public string Position { get; set; }
    
    /// <summary>
    /// Общая информация о человеке
    /// </summary>
    public virtual Person Person { get; set; }
}