using Application.Models;
using AutoMapper;
using DataStore.Models;

namespace Application.Mappers
{
    public class EmployeesStatisticsByRoleProfile:  Profile
    {
        public EmployeesStatisticsByRoleProfile()
        {
            CreateMap<EmployeesStatisticsByRole, EmployeesStatisticsByRoleDto>();
        }
    }
}