using System;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Commands
{
    public class UpdateEmployeeCommand
    {
        private readonly IEmployeesContext _employeesContext;
        public UpdateEmployeeCommand(IEmployeesContext employeesContext)
        {
            _employeesContext = employeesContext;
        }

        public async Task<bool> Execute(int id, string firstName, string lastName, DateTime birthDate, DateTime employmentDate,
            string homeAddress, int? bossId, Role role, decimal salary)
        {
            var employee = await _employeesContext.Employees.FirstAsync(x => x.Id == id);

                employee.FirstName = firstName;
                employee.LastName = lastName;
                employee.BirthDate = birthDate;
                employee.EmploymentDate = employmentDate;
                employee.HomeAddress = homeAddress;
                employee.BossId = bossId;
                employee.Role = role;
                employee.Salary = salary;

                await _employeesContext.SaveChangesAsync();
                return true;
            }
    }
}