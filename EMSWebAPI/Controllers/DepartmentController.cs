using AutoMapper;
using EMSWebAPI.Dto;
using EMSWebAPI.Interfaces;
using EMSWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
        public IActionResult GetDepartments()
        {
            var departments = mapper.Map<List<DepartmentDto>>(departmentRepository.GetDepartments());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Department))]
        [ProducesResponseType(400)]
        public IActionResult GetDepartmentById(int id)
        {
            if (!departmentRepository.IsDepartmentExists(id))
            {
                return NotFound();
            }

            var department = mapper.Map<DepartmentDto>(departmentRepository.GetDepartmentsById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(department);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Department))]
        [ProducesResponseType(400)]
        public IActionResult GetDepartmentByName(string name)
        {
            var department = mapper.Map<DepartmentDto>(departmentRepository.GetDepartmentsByName(name));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(department);
        }

        [HttpGet("employee/{id}")]
        [ProducesResponseType(200, Type = typeof(Department))]
        [ProducesResponseType(400)]
        public IActionResult GetDepatmentOfAnEmployee(int id)
        {
            var department = mapper.Map<DepartmentDto>(departmentRepository.GetDepartmentOfAnEmployee(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(department);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDepartment(DepartmentDto createdDepartment)
        {
            if (createdDepartment == null)
            {
                return BadRequest(ModelState);
            }

            var department = departmentRepository.GetDepartments()
                .Where(d => d.Name.Trim().ToUpper().Equals(createdDepartment.Name.Trim().ToUpper()))
                .FirstOrDefault();

            if (department != null)
            {
                ModelState.AddModelError("", "Department already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var departmentMap = mapper.Map<Department>(createdDepartment);

            if (!departmentRepository.CreateDepartment(departmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDepartment(int id, [FromBody] DepartmentDto updatedDepartment)
        {
            if (updatedDepartment == null || id != updatedDepartment.Id)
            {
                return BadRequest(ModelState);
            }

            if (!departmentRepository.IsDepartmentExists(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var departmentMap = mapper.Map<Department>(updatedDepartment);

            if (!departmentRepository.UpdateDepartment(departmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDepartment(int id)
        {
            if (!departmentRepository.IsDepartmentExists(id))
            {
                return BadRequest(ModelState);
            }

            var departmentToDelete = departmentRepository.GetDepartmentsById(id);

            if (!departmentRepository.DeleteDepartment(departmentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted.");
        }
    }
}
