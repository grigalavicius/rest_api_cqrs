using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using DataStore;
using DataStore.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetEmployeesStatisticsByRole
{
    public class GetStatisticsByRoleQueryHandler : IRequestHandler<GetStatisticsByRoleQuery, EmployeesStatisticsByRoleDto>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetStatisticsByRoleQueryHandler(ILoggerFactory loggerFactory, IMapper mapper, IEmployeesContext dbContext)
        {
            _logger = loggerFactory.CreateLogger<GetStatisticsByRoleQueryHandler>();
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<EmployeesStatisticsByRoleDto> Handle(GetStatisticsByRoleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = new StatisticsByRoleQuery(_dbContext.Employees);
                var employeesStatisticsByRole = await query.Execute(request.Role);
                var employeesStatisticsByRoleDto = _mapper.Map<EmployeesStatisticsByRoleDto>(employeesStatisticsByRole);
                return employeesStatisticsByRoleDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}