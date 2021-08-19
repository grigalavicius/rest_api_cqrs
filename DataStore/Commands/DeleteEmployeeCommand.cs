using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Commands
{
    public class DeleteEmployeeCommand
    {
        private readonly IEmployeesContext _employeesContext;
        public DeleteEmployeeCommand(IEmployeesContext employeesContext)
        {
            _employeesContext = employeesContext;
        }

        public async Task<bool> Execute(int id)
        {
            var employee = await _employeesContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null)
                throw new Exception($"Could not find employee by id: {id}");

            _employeesContext.Employees.Remove(employee);
            await _employeesContext.SaveChangesAsync();
            return true;
        }
    }
}