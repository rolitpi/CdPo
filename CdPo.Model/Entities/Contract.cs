using CdPo.Common.Entity;

namespace CdPo.Model.Entities;

/// <summary>
/// Договор обучающегося
/// </summary>
public class Contract: BaseEntity
{
    /// <summary>
    /// Обучающийся
    /// </summary>
    public virtual Student Student { get; set; }
    
    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Информация о договоре
    /// </summary>
    public string Description { get; set; }
}