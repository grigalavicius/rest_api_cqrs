using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Validation.ModelValidators;
using DataStore;
using MediatR;
using Microsoft.Extensions.Logging;
using StoreCmd = DataStore.Commands;

namespace Application.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, SuccessfullyExecutedModel>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly ILogger _logger;
        private readonly DeleteEmployeeCommandValidator _validator;

        public DeleteEmployeeCommandHandler(IEmployeesContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<DeleteEmployeeCommandHandler>();
            _validator = new DeleteEmployeeCommandValidator();
        }

        public async Task<SuccessfullyExecutedModel> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request);
                var command = new StoreCmd.DeleteEmployeeCommand(_dbContext);
                var successfullyExecuted = await command.Execute(request.Id);
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