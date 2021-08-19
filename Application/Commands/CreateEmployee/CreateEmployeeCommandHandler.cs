using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Validation.ModelValidators;
using AutoMapper;
using DataStore;
using DataStore.Models;
using DataStore.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using StoreCmd = DataStore.Commands;

namespace Application.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly CreateEmployeeCommandValidator _validator;

        public CreateEmployeeCommandHandler(IEmployeesContext dbContext, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<CreateEmployeeCommandHandler>();
            _validator = new CreateEmployeeCommandValidator();
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await Validate(request);
                var employeeId = await CreateNewEmployee(request);
                var employee = await GetEmployeeById(employeeId);
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return employeeDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task<Employee> GetEmployeeById(int employeeId)
        {
            var query = new EmployeeByIdQuery(_dbContext.Employees);
            var employee = await query.Execute(employeeId);
            return employee;
        }

        private async Task<int> CreateNewEmployee(CreateEmployeeCommand request)
        {
            var cmd = new StoreCmd.CreateEmployeeCommand(_dbContext);
            var newEmployee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                EmploymentDate = request.EmploymentDate,
                BossId = request.BossId,
                HomeAddress = request.HomeAddress,
                Salary = request.Salary,
                Role = request.Role,
            };

            var employeeId = await cmd.Execute(newEmployee);
            return employeeId;
        }

        private async Task Validate(CreateEmployeeCommand request)
        {
            await _validator.ValidateAndThrowAsync(request);
            if (request.Role == Role.Ceo && await _dbContext.EmployeeWithCeoRoleExist())
                throw new Exception("Ceo role already exist.");
        }
    }
}