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

namespace Application.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly UpdateEmployeeCommandValidator _validator;

        public UpdateEmployeeCommandHandler(IEmployeesContext dbContext, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<UpdateEmployeeCommandHandler>();
            _validator = new UpdateEmployeeCommandValidator();
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await Validate(request);
                var command = new StoreCmd.UpdateEmployeeCommand(_dbContext);
                var updatedSuccessfully = await command.Execute(request.Id, request.FirstName, request.LastName, request.BirthDate, request.EmploymentDate, request.HomeAddress,
                    request.BossId, request.Role, request.Salary);
                if (!updatedSuccessfully)
                    throw new Exception($"Failed to update employee by id: {request.Id}");

                var employeeAfterUpdate = await GetEmployeeById(request.Id);
                var employeeDto = _mapper.Map<EmployeeDto>(employeeAfterUpdate);

                return employeeDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task Validate(UpdateEmployeeCommand request)
        {
            await _validator.ValidateAndThrowAsync(request);
            
            var employee = await GetEmployeeById(request.Id);
            if (employee is null)
            {
                throw new Exception(string.Format(ValidationMessages.EmployeeDoesNotExistMessage, request.Id));
            }

            if (employee.Role != Role.Ceo && request.Role == Role.Ceo && await _dbContext.EmployeeWithCeoRoleExist())
                throw new Exception(ValidationMessages.EmployeeWithCeoRoleAlreadyExist);
        }

        private async Task<Employee> GetEmployeeById(int id)
        {
            var query = new EmployeeByIdQuery(_dbContext.Employees);
            var employee = await query.Execute(id);
            return employee;
        }
    }
}