using CdPo.Common.Entity;

namespace CdPo.Model.Entities;

/// <summary>
/// Сотрудник
/// </summary>
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