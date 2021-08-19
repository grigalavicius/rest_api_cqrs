using System;
using Application.Models;
using DataStore.Models;
using MediatR;

namespace Application.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<EmployeeDto>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; } 
        public DateTime BirthDate { get; init; }
        public DateTime EmploymentDate { get; init; }
        public int? BossId { get; init; }
        public string HomeAddress { get; init; }
        public decimal Salary { get; init; }
        public Role Role { get; init; }
    }
}