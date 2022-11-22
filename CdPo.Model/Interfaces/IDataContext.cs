using CdPo.Model.Entities;

using Microsoft.EntityFrameworkCore;

namespace CdPo.Model.Interfaces;

/// <summary>
/// Интерфейс для работы с бд
/// </summary>
public interface IDataContext
{
    /// <summary>
    /// Таблица персон
    /// </summary>
    DbSet<Person> Persons { get; set; }
    
    /// <summary>
    /// Таблица сотрудников
    /// </summary>
    DbSet<Staff> Staffs { get; set; }
    
    /// <summary>
    /// Таблица обучающихся
    /// </summary>
    DbSet<Student> Students { get; set; }
    
    /// <summary>
    /// Таблица договоров обучающихся
    /// </summary>
    DbSet<Contract> Contracts { get; set; }
    
    /// <summary>
    /// Таблица дисциплин
    /// </summary>
    DbSet<Discipline> Disciplines { get; set; }
    
    /// <summary>
    /// Таблица курсов
    /// </summary>
    DbSet<TrainingCourse> TrainingCourses { get; set; }
    
    /// <summary>
    /// Таблица груп
    /// </summary>
    DbSet<Group> Groups { get; set; }
    
    /// <summary>
    /// Таблица файлов
    /// </summary>
    DbSet<FileMetadata> Files { get; set; }
}