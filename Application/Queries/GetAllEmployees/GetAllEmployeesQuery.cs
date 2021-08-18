using System.Collections.Generic;
using Application.Models;
using MediatR;

namespace Application.Queries.GetAllEmployees
{
    public class GetAllEmployeesQuery : IRequest<IReadOnlyCollection<EmployeeDto>>
    {
    }
}