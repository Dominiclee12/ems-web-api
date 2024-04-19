using EMSWebAPI.Data;
using EMSWebAPI.Models;

namespace EMSWebAPI
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.EmployeesProjects.Any())
            {
                var employeesProjects = new List<EmployeeProject>()
                {
                    new EmployeeProject() {
                        Employee = new Employee()
                        {
                            Name = "John Doe",
                            Position = "Software Engineer",
                            Department = new Department() { Name = "Engineering" }
                        },
                        Project = new Project()
                        {
                            Name = "Website Redesign"
                        }
                    },
                    new EmployeeProject() {
                        Employee = new Employee()
                        {
                            Name = "Jane Smith",
                            Position = "QA Engineer",
                            Department = new Department() { Name = "Quality Assurance" }
                        },
                        Project = new Project()
                        {
                            Name = "Product Testing"
                        }
                    },
                    new EmployeeProject() {
                        Employee = new Employee()
                        {
                            Name = "Michael Johnson",
                            Position = "Product Manager",
                            Department = new Department() { Name = "Marketing" }
                        },
                        Project = new Project()
                        {
                            Name = "Mobile App Development"
                        }
                    }
                };

                dataContext.EmployeesProjects.AddRange(employeesProjects);
                dataContext.SaveChanges();
            }
        }
    }
}
