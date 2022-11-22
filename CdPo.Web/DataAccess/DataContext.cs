using CdPo.Common.Entity;
using CdPo.Model.Entities;
using CdPo.Model.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CdPo.Web.DataAccess;

/// <inheritdoc cref="IDataContext"/>
public class DataContext: DbContext, IDataContext
{
    public DataContext(DbContextOptions options) : base(options) {}

    public DbSet<Person> Persons { get; set; }
    
    public DbSet<Staff> Staffs { get; set; }
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Contract> Contracts { get; set; }
    
    public DbSet<Discipline> Disciplines { get; set; }
    
    public DbSet<TrainingCourse> TrainingCourses { get; set; }
    
    public DbSet<Group> Groups { get; set; }
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