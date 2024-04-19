using EMSWebAPI.Data;
using EMSWebAPI.Models;

namespace EMSWebAPI.Interfaces
{
    public interface IProjectRepository
    {
        ICollection<Project> GetProjects();
        Project GetProjectById(int id);
        Project GetProjectByName(string name);
        bool CreateProject(Project project);
        bool UpdateProject(Project project);
        bool DeleteProject(Project project);
        bool IsProjectExists(int id);
        bool Save();
    }
}
