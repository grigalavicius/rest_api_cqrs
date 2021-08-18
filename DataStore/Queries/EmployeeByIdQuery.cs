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
                .FirstOrDefaultAsync(x => x.Id == id);

            return employees;
        }
    }
}