using System.ComponentModel.DataAnnotations.Schema;

using CdPo.Common.Entity;
using CdPo.Model.Attributes;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Annotations;

namespace CdPo.Model.Entities;

/// <summary>
/// Обучающийся
/// </summary>
[GeneratedController("student", "Обучающиеся")]
[Index(nameof(PersonId), IsUnique = true)]
public class Student: BaseEntity
{
    /// <summary>
    /// Общая информация о человеке
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    internal Person Person { get; set; }
    
    [ForeignKey(nameof(Person))]
    [SwaggerSchema(WriteOnly = true)]
    public long PersonId { get; set; }
    
    /// <summary>
    /// Группа, в которой состоит обучающийся
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    internal Group Group { get; set; }
    
    [ForeignKey(nameof(Group))]
    [SwaggerSchema(WriteOnly = true)]
    public long GroupId { get; set; }
}