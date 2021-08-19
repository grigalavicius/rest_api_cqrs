using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using DataStore;
using DataStore.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetEmployeesByBossId
{
    public class GetEmployeesByBossIdQueryHandler : IRequestHandler<GetEmployeesByBossIdQuery, IReadOnlyCollection<EmployeeDto>>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetEmployeesByBossIdQueryHandler(ILoggerFactory loggerFactory, IMapper mapper, IEmployeesContext dbContext)
        {
            _logger = loggerFactory.CreateLogger<GetEmployeesByBossIdQueryHandler>();
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<EmployeeDto>> Handle(GetEmployeesByBossIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = new EmployeesByBossIdQuery(_dbContext.Employees);
                var employees = await query.Execute(request.BossId);
                var resultList = employees.Select(x => _mapper.Map<EmployeeDto>(x)).ToList();
                return resultList;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}