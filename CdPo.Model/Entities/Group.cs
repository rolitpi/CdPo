using CdPo.Common.Entity;
using CdPo.Common.Enum;
using CdPo.Model.Attributes;

using Swashbuckle.AspNetCore.Annotations;

namespace CdPo.Model.Entities;

/// <summary>
/// Студенческая группа
/// </summary>
[GeneratedController("group", "Группы")]
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

    /// <summary>
    /// Студенты группы
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public List<Student> Students { get; set; }
}