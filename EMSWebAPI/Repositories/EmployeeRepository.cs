using EMSWebAPI.Data;
using EMSWebAPI.Interfaces;
using EMSWebAPI.Models;

namespace EMSWebAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext context;

        public EmployeeRepository(DataContext context)
        {
            this.context = context;
        }

        public ICollection<Employee> GetEmployees()
        {
            return context.Employees.OrderBy(e => e.Id).ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return context.Employees.Where(e => e.Id == id).FirstOrDefault();
        }

        public Employee GetEmployeeByName(string name)
        {
            return context.Employees.Where(e => e.Name.Contains(name)).FirstOrDefault();
        }

        public ICollection<Employee> GetEmployeesByDepartment(int departmentId)
        {
            return context.Employees.OrderBy(e => e.Id).Where(e => e.Department.Id == departmentId).ToList();
        }

        public ICollection<Employee> GetEmployeesOfAProject(int projectId)
        {
            return context.EmployeesProjects.Where(ep => ep.ProjectId == projectId).Select(e => e.Employee).ToList();
        }

        public bool CreateEmployee(int projectId, Employee employee)
        {
            var project = context.Projects.Where(p => p.Id == projectId).FirstOrDefault();

            var employeeProject = new EmployeeProject()
            {
                Employee = employee,
                Project = project
            };

            context.Add(employeeProject);
            
            context.Add(employee);
            return Save();
        }

        public bool UpdateEmployee(Employee employee)
        {
            context.Update(employee);
            return Save();
        }

        public bool DeleteEmployee(Employee employee)
        {
            context.Remove(employee);
            return Save();
        }

        public bool IsEmployeeExists(int id)
        {
            return context.Employees.Any(e => e.Id == id);
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
