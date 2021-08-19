using System.Threading.Tasks;
using DataStore.Models;

namespace DataStore.Commands
{
    public class CreateEmployeeCommand
    {
        private readonly IEmployeesContext _employeesContext;

        public CreateEmployeeCommand(IEmployeesContext employeesContext)
        {
            _employeesContext = employeesContext;
        }

        public async Task<int> Execute(Employee employee)
        {
            await _employeesContext.Employees.AddAsync(employee);
            await _employeesContext.SaveChangesAsync();
            return employee.Id;
        }
    }
}