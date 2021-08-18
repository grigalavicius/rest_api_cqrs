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

namespace Application.Queries.GetAllEmployees
{
    internal class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyCollection<EmployeeDto>>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetAllEmployeesQueryHandler(IEmployeesContext dbContext, ILoggerFactory loggerFactory, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<GetAllEmployeesQueryHandler>();
        }

        public async Task<IReadOnlyCollection<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = new AllEmployeesQuery(_dbContext.Employees);
                var employees = await query.Execute();
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