using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using DataStore;
using DataStore.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        
        public GetEmployeeByIdQueryHandler(IEmployeesContext dbContext, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<GetEmployeeByIdQueryHandler>();
        }


        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = new EmployeeByIdQuery(_dbContext.Employees);
                var employee = await query.Execute(request.Id);
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return employeeDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}