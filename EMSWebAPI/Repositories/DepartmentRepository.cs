using EMSWebAPI.Data;
using EMSWebAPI.Interfaces;
using EMSWebAPI.Models;
using System.Runtime.Serialization.Formatters;

namespace EMSWebAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext context;

        public DepartmentRepository(DataContext context)
        {
            this.context = context;
        }

        public ICollection<Department> GetDepartments()
        {
            return context.Departments.OrderBy(d => d.Id).ToList();
        }

        public Department GetDepartmentsById(int id)
        {
            return context.Departments.Where(d => d.Id == id).FirstOrDefault();
        }

        public Department GetDepartmentsByName(string name)
        {
            return context.Departments.Where(d => d.Name.Contains(name)).FirstOrDefault();
        }

        public Department GetDepartmentOfAnEmployee(int employeeId)
        {
            return context.Employees.Where(e => e.Id == employeeId).Select(d => d.Department).FirstOrDefault();
        }

        public bool CreateDepartment(Department department)
        {
            context.Add(department);
            return Save();
        }

        public bool IsDepartmentExists(int id)
        {
            return context.Departments.Any(d => d.Id == id);
        }

        public bool UpdateDepartment(Department department)
        {
            context.Update(department);
            return Save();
        }

        public bool DeleteDepartment(Department department)
        {
            context.Remove(department);
            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
