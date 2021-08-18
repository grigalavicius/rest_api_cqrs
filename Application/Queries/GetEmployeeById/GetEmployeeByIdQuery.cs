using Application.Models;
using MediatR;

namespace Application.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}