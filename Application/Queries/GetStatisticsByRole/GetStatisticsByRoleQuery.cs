using Application.Models;
using DataStore.Models;
using MediatR;

namespace Application.Queries.GetStatisticsByRole
{
    public class GetStatisticsByRoleQuery : IRequest<EmployeesStatisticsByRoleDto>
    {
        public GetStatisticsByRoleQuery(Role role)
        {
            Role = role;
        }

        public Role Role { get; }
        
    }
}