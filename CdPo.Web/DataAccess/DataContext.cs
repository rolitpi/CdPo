using CdPo.Common.Entity;
using CdPo.Model.Entities;
using CdPo.Model.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CdPo.Web.DataAccess;

/// <inheritdoc cref="IDataContext"/>
public class DataContext: DbContext
{
    public DataContext(DbContextOptions options) : base(options) {}

    /// <summary>
    /// Таблица персон
    /// </summary>
    public DbSet<Person> Persons { get; set; }
    
    /// <summary>
    /// Таблица сотрудников
    /// </summary>
    public DbSet<Staff> Staffs { get; set; }
    
    /// <summary>
    /// Таблица обучающихся
    /// </summary>
    public DbSet<Student> Students { get; set; }
    
    /// <summary>
    /// Таблица договоров обучающихся
    /// </summary>
    public DbSet<Contract> Contracts { get; set; }
    
    /// <summary>
    /// Таблица дисциплин
    /// </summary>
    public DbSet<Discipline> Disciplines { get; set; }
    
    /// <summary>
    /// Таблица курсов
    /// </summary>
    public DbSet<TrainingCourse> TrainingCourses { get; set; }
    
    /// <summary>
    /// Таблица груп
    /// </summary>
    public DbSet<Group> Groups { get; set; }
    
    /// <summary>
    /// Таблица файлов
    /// </summary>
    public DbSet<FileMetadata> Files { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
        {
            modelBuilder
                .Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.ObjectCreateDate))
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.ObjectEditDate))
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
            modelBuilder
                .Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.ObjectVersion))
                .HasDefaultValueSql("0")
                .ValueGeneratedOnAdd();
        }
    }
}