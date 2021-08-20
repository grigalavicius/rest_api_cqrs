using System.Linq;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Queries
{
    public class DoesEmployeeWithCeoRoleAlreadyExistQuery
    {
        private readonly IQueryable<Employee> _employeesDb;

        public DoesEmployeeWithCeoRoleAlreadyExistQuery(IQueryable<Employee> employeesDb)
        {
            _employeesDb = employeesDb;
        }

        public async Task<bool> Execute()
        {
            return await _employeesDb.AnyAsync(x => x.Role == Role.Ceo);
        }
    }
}