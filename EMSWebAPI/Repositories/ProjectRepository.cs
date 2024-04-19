using EMSWebAPI.Data;
using EMSWebAPI.Interfaces;
using EMSWebAPI.Models;

namespace EMSWebAPI.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext context;

        public ProjectRepository(DataContext context)
        {
            this.context = context;
        }

        public ICollection<Project> GetProjects()
        {
            return context.Projects.OrderBy(p => p.Id).ToList();
        }

        public Project GetProjectById(int id)
        {
            return context.Projects.Where(p => p.Id == id).FirstOrDefault();
        }

        public Project GetProjectByName(string name)
        {
            return context.Projects.Where(p => p.Name.Contains(name)).FirstOrDefault();
        }

        public bool CreateProject(Project project)
        {
            context.Add(project);
            return Save();
        }

        public bool UpdateProject(Project project)
        {
            context.Update(project);
            return Save();
        }

        public bool DeleteProject(Project project)
        {
            context.Remove(project);
            return Save();
        }

        public bool IsProjectExists(int id)
        {
            return context.Projects.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved =context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
