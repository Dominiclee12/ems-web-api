using AutoMapper;
using EMSWebAPI.Dto;
using EMSWebAPI.Interfaces;
using EMSWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace EMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetEmployees()
        {
            var employees = mapper.Map<List<EmployeeDto>>(employeeRepository.GetEmployees());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(400)]
        public IActionResult GetEmployeeById(int id)
        {
            if (!employeeRepository.IsEmployeeExists(id))
            {
                return NotFound();
            }

            var employee = mapper.Map<EmployeeDto>(employeeRepository.GetEmployeeById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(employee);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        [ProducesResponseType(400)]
        public IActionResult GetEmployeeByName(string name)
        {
            var employee = mapper.Map<EmployeeDto>(employeeRepository.GetEmployeeByName(name));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(employee);
        }

        [HttpGet("department/{id}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(400)]
        public IActionResult GetEmployeesByDepartment(int id)
        {
            var employees = mapper.Map<List<EmployeeDto>>(employeeRepository.GetEmployeesByDepartment(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(employees);
        }

        [HttpGet("project/{id}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(400)]
        public IActionResult GetEmployeesOfAProject(int id)
        {
            var employees = mapper.Map<List<EmployeeDto>>(employeeRepository.GetEmployeesOfAProject(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(employees);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateEmployee([FromQuery] int depId, [FromQuery] int proId, [FromBody] EmployeeDto createdEmployee)
        {
            if (createdEmployee == null)
            {
                return BadRequest(ModelState);
            }

            var employee = employeeRepository.GetEmployees()
                .Where(e => e.Name.Trim().ToUpper() == createdEmployee.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (employee != null)
            {
                ModelState.AddModelError("", "Employee already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeMap = mapper.Map<Employee>(createdEmployee);
            employeeMap.Department = departmentRepository.GetDepartmentsById(depId);

            if (!employeeRepository.CreateEmployee(proId, employeeMap))
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
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDto updatedEmployee)
        {
            if (updatedEmployee == null || id != updatedEmployee.Id)
            {
                return BadRequest(ModelState);
            }

            if (!employeeRepository.IsEmployeeExists(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var employeeMap = mapper.Map<Employee>(updatedEmployee);

            if (!employeeRepository.UpdateEmployee(employeeMap))
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
        public IActionResult DeleteEmployee(int id)
        {
            if (!employeeRepository.IsEmployeeExists(id))
            {
                return NotFound();
            }

            var employeeToDelete = employeeRepository.GetEmployeeById(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!employeeRepository.DeleteEmployee(employeeToDelete))
            {
                ModelState.AddModelError("", "Something wrong when deleting.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted.");
        }
    }
}
