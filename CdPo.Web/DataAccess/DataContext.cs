using CdPo.Model.Entities;

using Microsoft.EntityFrameworkCore;

namespace CdPo.Web.DataAccess;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions options) : base(options) {}
    
    public DbSet<Person> Persons { get; set; }
    
    public DbSet<Staff> Staffs { get; set; }
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Contract> Contracts { get; set; }
    
    public DbSet<Discipline> Disciplines { get; set; }
    
    public DbSet<TrainingCourse> TrainingCourses { get; set; }
    
    public DbSet<Group> Groups { get; set; }
}