using EMSWebAPI.Models;

namespace EMSWebAPI.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetEmployees();
        Employee GetEmployeeById(int id);
        Employee GetEmployeeByName(string name);
        ICollection<Employee> GetEmployeesByDepartment(int departmentId);
        ICollection<Employee> GetEmployeesOfAProject(int projectId);
        bool IsEmployeeExists(int id);
        bool CreateEmployee(int projectId, Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        bool Save();
    }
}
