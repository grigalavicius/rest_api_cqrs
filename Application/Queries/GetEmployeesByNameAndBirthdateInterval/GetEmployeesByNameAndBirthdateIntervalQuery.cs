using System;
using System.Collections.Generic;
using Application.Models;
using MediatR;

namespace Application.Queries.GetEmployeesByNameAndBirthdateInterval
{
    public class GetEmployeesByNameAndBirthdateIntervalQuery : IRequest<IReadOnlyCollection<EmployeeDto>>
    {
        public GetEmployeesByNameAndBirthdateIntervalQuery(string name, DateTime from, DateTime to)
        {
            Name = name;
            From = from;
            To = to;
        }

        public string Name { get; }
        public DateTime From { get; }
        public DateTime To { get; }
    }
}