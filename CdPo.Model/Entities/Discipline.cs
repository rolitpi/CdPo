using CdPo.Common.Entity;
using CdPo.Common.Enum;
using CdPo.Model.Attributes;

namespace CdPo.Model.Entities;

/// <summary>
/// Дисциплина/предмет
/// </summary>
[GeneratedController("discipline", "Предметы")]
public class Discipline: BaseEntity
{
    /// <summary>
    /// Наименование дисциплины
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Продолжительность дисциплины в часах
    /// </summary>
    public int Duration { get; set; }
    
    /// <summary>
    /// Форма контроля
    /// </summary>
    public ControlForm ControlForm { get; set; }
}