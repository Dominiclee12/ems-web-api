using AutoMapper;
using EMSWebAPI.Dto;
using EMSWebAPI.Interfaces;
using EMSWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public ProjectController(IProjectRepository projectRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            this.projectRepository = projectRepository;
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        public IActionResult GetProjects()
        {
            var projects = mapper.Map<List<ProjectDto>>(projectRepository.GetProjects());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(projects);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Project))]
        [ProducesResponseType(400)]
        public IActionResult GetProjectById(int id)
        {
            if (!projectRepository.IsProjectExists(id))
            {
                return NotFound();
            }

            var project = mapper.Map<ProjectDto>(projectRepository.GetProjectById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(project);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Project))]
        [ProducesResponseType(400)]
        public IActionResult GetProjectByName(string name)
        {
            var project = mapper.Map<ProjectDto>(projectRepository.GetProjectByName(name));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(project);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProject([FromBody] ProjectDto createdProject)
        {
            if (createdProject == null)
            {
                return BadRequest(ModelState);
            }

            var project = projectRepository.GetProjects()
                .Where(e => e.Name.Trim().ToUpper() == createdProject.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (project != null)
            {
                ModelState.AddModelError("", "Project already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectMap = mapper.Map<Project>(createdProject);
            //projectMap.AssignBy = employeeRepository.GetEmployeeById(empId);

            if (!projectRepository.CreateProject(projectMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully created.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProject(int id, [FromBody] ProjectDto updatedProject)
        {
            if (updatedProject == null || id != updatedProject.Id)
            {
                return BadRequest(ModelState);
            }

            if (!projectRepository.IsProjectExists(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var projectMap = mapper.Map<Project>(updatedProject);

            if (!projectRepository.UpdateProject(projectMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating.");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully updated.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProject(int id)
        {
            if (!projectRepository.IsProjectExists(id))
            {
                return NotFound();
            }

            var projectToDelete = projectRepository.GetProjectById(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!projectRepository.DeleteProject(projectToDelete))
            {
                ModelState.AddModelError("", "Something wrong when deleting.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted.");
        }
    }
}
