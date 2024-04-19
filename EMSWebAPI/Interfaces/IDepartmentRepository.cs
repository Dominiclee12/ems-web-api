using EMSWebAPI.Models;

namespace EMSWebAPI.Interfaces
{
    public interface IDepartmentRepository
    {
        ICollection<Department> GetDepartments();
        Department GetDepartmentsById(int id);
        Department GetDepartmentsByName(string name);
        Department GetDepartmentOfAnEmployee(int employeeId);
        bool IsDepartmentExists(int id);
        bool CreateDepartment(Department department);
        bool UpdateDepartment(Department department);
        bool DeleteDepartment(Department department);
        bool Save();
    }
}
