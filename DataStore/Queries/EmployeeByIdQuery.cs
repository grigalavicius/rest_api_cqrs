using System.Linq;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Queries
{
    public class EmployeeByIdQuery
    {
        private readonly IQueryable<Employee> _employeesDb;

        public EmployeeByIdQuery(IQueryable<Employee> employeesDb)
        {
            _employeesDb = employeesDb;
        }
        
        public async Task<Employee> Execute(int id)
        {
            var employees = await _employeesDb
                .Include(x => x.Boss)
                .Include(x => x.Employees)
                .FirstOrDefaultAsync(x => x.Id == id);

            return employees;
        }
    }
}