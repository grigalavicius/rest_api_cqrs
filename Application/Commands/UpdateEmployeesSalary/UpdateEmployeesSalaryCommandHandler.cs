using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Validation.ModelValidators;
using DataStore;
using MediatR;
using Microsoft.Extensions.Logging;
using StoreCmd = DataStore.Commands;

namespace Application.Commands.UpdateEmployeesSalary
{
    public class UpdateEmployeesSalaryCommandHandler : IRequestHandler<UpdateEmployeesSalaryCommand, SuccessfullyExecutedModel>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly ILogger _logger;
        private readonly UpdateEmployeesSalaryCommandValidator _validator;

        public UpdateEmployeesSalaryCommandHandler(IEmployeesContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<UpdateEmployeesSalaryCommandHandler>();
            _validator = new UpdateEmployeesSalaryCommandValidator();
        }

        public async Task<SuccessfullyExecutedModel> Handle(UpdateEmployeesSalaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request);

                var command = new StoreCmd.UpdateEmployeesSalaryCommand(_dbContext);
                var successfullyExecuted = await command.Execute(request.Id, request.Salary);
                return new SuccessfullyExecutedModel(successfullyExecuted);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}