using CdPo.Common.Entity;
using CdPo.Model.Attributes;

namespace CdPo.Model.Entities;

/// <summary>
/// Договор обучающегося
/// </summary>
[GeneratedController("contract", "Договоры")]
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