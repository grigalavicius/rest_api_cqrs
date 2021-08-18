using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Affecto.Mapping;
using Application.Models;
using Application.Queries.GetAllEmployees;
using Application.Queries.GetEmployeeById;
using Application.Queries.GetEmployeesByNameAndBirthdateInterval;
using Application.Queries.GetStatisticsByRole;
using DataStore.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RestApiTask.Controllers
{
    /// <summary>
    ///
    /// </summary>
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
            if (!Enum.TryParse(typeof(Role), role, true, out var roleEnum))
                throw new Exception("Invalid role input");
            
            var query = new GetStatisticsByRoleQuery((Role)roleEnum);
            var employeesStatisticsByRole = await _mediator.Send(query);
            
            return Ok(employeesStatisticsByRole);
        }
    }
}