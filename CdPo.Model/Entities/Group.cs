using CdPo.Common.Entity;
using CdPo.Common.Enum;

namespace CdPo.Model.Entities;

/// <summary>
/// Студенческая группа
/// </summary>
public class Group: BaseEntity
{
    /// <summary>
    /// Номер группы
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Форма обучения группы
    /// </summary>
    public EducationForm EducationForm { get; set; }
}