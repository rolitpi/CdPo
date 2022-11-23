using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CdPo.Common.Entity;
using CdPo.Model.Attributes;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Annotations;

namespace CdPo.Model.Entities;

/// <summary>
/// Договор обучающегося
/// </summary>
[GeneratedController("contract", "Договоры")]
[Index(nameof(StudentId), IsUnique = true)]
public class Contract: BaseEntity
{
    /// <summary>
    /// Обучающийся
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public Student Student { get; set; }
    
    [ForeignKey(nameof(Student))]
    [SwaggerSchema(WriteOnly = true)]
    public long StudentId { get; set; }
    
    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Информация о договоре
    /// </summary>
    public string Description { get; set; }
}