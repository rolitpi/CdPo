using System.ComponentModel.DataAnnotations.Schema;

using CdPo.Common.Entity;
using CdPo.Model.Attributes;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Annotations;

namespace CdPo.Model.Entities;

/// <summary>
/// Сотрудник
/// </summary>
[GeneratedController("staff", "Сотрудники")]
[Index(nameof(PersonId), IsUnique = true)]
public class Staff: BaseEntity
{
    /// <summary>
    /// Должность
    /// </summary>
    public string Position { get; set; }
    
    /// <summary>
    /// Общая информация о человеке
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    internal Person Person { get; set; }
    
    [ForeignKey(nameof(Person))]
    [SwaggerSchema(WriteOnly = true)]
    public long PersonId { get; set; }
}