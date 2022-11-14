using CdPo.Common.Entity;

namespace CdPo.Model.Entities;

/// <summary>
/// Человек
/// </summary>
public class Person: BaseEntity
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Отчество
    /// </summary>
    public string Patronymic { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? BirthDate { get; set; }
}