using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Validation;
using Application.Validation.ModelValidators;
using AutoMapper;
using DataStore;
using DataStore.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetEmployeesByNameAndBirthdateInterval
{
    public class GetEmployeesByNameAndBirthdateIntervalQueryHandler : IRequestHandler<GetEmployeesByNameAndBirthdateIntervalQuery, IReadOnlyCollection<EmployeeDto>>
    {
        private readonly IEmployeesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly GetEmployeesByNameAndBirthdateIntervalQueryValidator _validator;
        private readonly ILogger _logger;

        public GetEmployeesByNameAndBirthdateIntervalQueryHandler(IEmployeesContext dbContext, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = new GetEmployeesByNameAndBirthdateIntervalQueryValidator();
            _logger = loggerFactory.CreateLogger<GetEmployeesByNameAndBirthdateIntervalQueryHandler>();
        }


        public async Task<IReadOnlyCollection<EmployeeDto>> Handle(GetEmployeesByNameAndBirthdateIntervalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request);
                var query = new EmployeesByNameAndBirthdateIntervalQuery(_dbContext.Employees);
                var employees = await query.Execute(request.Name, request.From, request.To);
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