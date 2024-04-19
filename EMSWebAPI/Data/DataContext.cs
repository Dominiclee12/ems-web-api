using EMSWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMSWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
       
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeesProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.EmployeeId, ep.ProjectId });
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(e => e.Employee)
                .WithMany(ep => ep.EmployeesProjects)   
                .HasForeignKey(e => e.EmployeeId);
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(p => p.Project)
                .WithMany(ep => ep.EmployeesProjects)
                .HasForeignKey(p => p.ProjectId);
        }
    }
}
