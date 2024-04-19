namespace EMSWebAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<EmployeeProject> EmployeesProjects { get; set; }
    }
}
