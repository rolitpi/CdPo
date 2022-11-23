using System.ComponentModel.DataAnnotations.Schema;

using CdPo.Common.Entity;
using CdPo.Model.Attributes;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Annotations;

namespace CdPo.Model.Entities;

/// <summary>
/// Учебный курс
/// </summary>
[GeneratedController("training_course", "Учебные курсы")]
[Index(nameof(TeacherId), nameof(GroupId), nameof(DisciplineId), IsUnique = true)]
public class TrainingCourse: BaseEntity
{
    /// <summary>
    /// Группа обучающихся
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public Group Group { get; set; }
    
    [ForeignKey(nameof(Group))]
    [SwaggerSchema(WriteOnly = true)]
    public long GroupId { get; set; }
    
    /// <summary>
    /// Преподаватель
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public Staff Teacher { get; set; }
    
    [ForeignKey(nameof(Teacher))]
    [SwaggerSchema(WriteOnly = true)]
    public long TeacherId { get; set; }
    
    /// <summary>
    /// Дисциплина курса
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public Discipline Discipline { get; set; }
    
    [ForeignKey(nameof(Discipline))]
    [SwaggerSchema(WriteOnly = true)]
    public long DisciplineId { get; set; }
    
    /// <summary>
    /// Дата начала обучения
    /// </summary>
    public DateTime TrainingStartDate { get; set; }
}