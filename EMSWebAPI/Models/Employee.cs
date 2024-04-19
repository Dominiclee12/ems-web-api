namespace EMSWebAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public Department Department { get; set; }
        public ICollection<EmployeeProject> EmployeesProjects { get; set; }
    }
}
