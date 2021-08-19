using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Commands.CreateEmployee;
using Application.Commands.DeleteEmployee;
using Application.Commands.UpdateEmployee;
using Application.Commands.UpdateEmployeesSalary;
using Application.Models;
using Application.Queries.GetAllEmployees;
using Application.Queries.GetEmployeeById;
using Application.Queries.GetEmployeesByBossId;
using Application.Queries.GetEmployeesByNameAndBirthdateInterval;
using Application.Queries.GetEmployeesStatisticsByRole;
using DataStore.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestApiTask.Commands;
using AppCmd = Application.Commands;

namespace RestApiTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get a list of all existing employees
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), 200)]
        [Produces("application/json")]
        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var getAllEmployeesQuery = new GetAllEmployeesQuery();
            var getAllEmployeesList = await _mediator.Send(getAllEmployeesQuery);

            return Ok(getAllEmployeesList);
        }
        
        /// <summary>
        /// Get a list of all employees by boss id
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), 200)]
        [Produces("application/json")]
        [HttpGet]
        [Route(nameof(GetByBossId))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetByBossId([FromQuery] int? bossId)
        {
            var query = new GetEmployeesByBossIdQuery(bossId);
            var employees = await _mediator.Send(query);

            return Ok(employees);
        }

        /// <summary>
        /// Get an employee by its id
        /// </summary>
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        [Produces("application/json")]
        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<ActionResult<EmployeeDto>> GetById([FromQuery] int id)
        {
            var getEmployeeByIdQuery = new GetEmployeeByIdQuery(id);
            var employee = await _mediator.Send(getEmployeeByIdQuery);

            return Ok(employee);
        }

        /// <summary>
        /// Search for employees by name and birthdate interval
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), 200)]
        [Produces("application/json")]
        [HttpGet]
        [Route(nameof(GetByNameAndBirthdateInterval))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetByNameAndBirthdateInterval([FromQuery] string name, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var query = new GetEmployeesByNameAndBirthdateIntervalQuery(name, from, to);
            var employees = await _mediator.Send(query);

            return Ok(employees);
        }

        /// <summary>
        /// Getting employee count and average salary for particular Role
        /// </summary>
        [ProducesResponseType(typeof(EmployeesStatisticsByRoleDto), 200)]
        [Produces("application/json")]
        [HttpGet]
        [Route(nameof(GetStatisticsByRole))]
        public async Task<ActionResult<EmployeesStatisticsByRoleDto>> GetStatisticsByRole([FromQuery] string role)
        {
            if (!Enum.TryParse(typeof(Role), role, true, out var roleEnum) || roleEnum is null)
                throw new Exception("Invalid role input");

            var query = new GetStatisticsByRoleQuery((Role)roleEnum);
            var employeesStatisticsByRole = await _mediator.Send(query);

            return Ok(employeesStatisticsByRole);
        }

        /// <summary>
        /// Adding new employee
        /// </summary>
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        [Produces("application/json")]
        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDtoCmd command)
        {
            var createEmployeeCommand = new CreateEmployeeCommand
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                BirthDate = command.BirthDate,
                EmploymentDate = command.EmploymentDate,
                BossId = command.BossId,
                HomeAddress = command.HomeAddress,
                Salary = command.Salary,
                Role = command.Role
            };

            var employee = await _mediator.Send(createEmployeeCommand);
            return Ok(employee);
        }

        /// <summary>
        /// Modify properties of an existing employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="command">Employee update command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        [Produces("application/json")]
        [HttpPut]
        [Route(nameof(Update))]
        public async Task<ActionResult<EmployeeDto>> Update([FromQuery] int employeeId, [FromBody] UpdateEmployeeDtoCmd command)
        {
            var employeeDto = await _mediator.Send(new UpdateEmployeeCommand
            {
                Id = employeeId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                BirthDate = command.BirthDate,
                EmploymentDate = command.EmploymentDate,
                BossId = command.BossId,
                HomeAddress = command.HomeAddress,
                Salary = command.Salary,
                Role = command.Role
            });
            
            return Ok(employeeDto);
        }
        
        /// <summary>
        /// Updates employees' salary
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="command">Employee salary update command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(SuccessfullyExecutedModel), 200)]
        [Produces("application/json")]
        [HttpPut]
        [Route(nameof(UpdateSalary))]
        public async Task<ActionResult<SuccessfullyExecutedModel>> UpdateSalary([FromQuery] int employeeId, [FromBody] UpdateEmployeeSalaryDtoCmd command)
        {
            var result = await _mediator.Send(new UpdateEmployeesSalaryCommand(employeeId, command.Salary));
            return Ok(result);
        }
        
        /// <summary>
        /// Deletes employee by it's id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(SuccessfullyExecutedModel), 200)]
        [Produces("application/json")]
        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<ActionResult<SuccessfullyExecutedModel>> Delete([FromQuery] int employeeId)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(employeeId));
            return Ok(result);
        }
    }
}