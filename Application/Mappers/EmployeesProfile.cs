using Application.Models;
using AutoMapper;
using DataStore.Models;

namespace Application.Mappers
{
    internal class EmployeesProfile:  Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .MaxDepth(2);
        }
    }
}