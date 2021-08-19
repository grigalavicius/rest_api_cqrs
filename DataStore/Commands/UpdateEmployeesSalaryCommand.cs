using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Commands
{
    public class UpdateEmployeesSalaryCommand
    {
        private readonly IEmployeesContext _employeesContext;
        public UpdateEmployeesSalaryCommand(IEmployeesContext employeesContext)
        {
            _employeesContext = employeesContext;
        }

        public async Task<bool> Execute(int id, decimal salary)
        {
            var employee = await _employeesContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null)
                throw new Exception($"Could not find employee by id: {id}");

            employee.Salary = salary;
            await _employeesContext.SaveChangesAsync();
            return true;
        }
    }
}