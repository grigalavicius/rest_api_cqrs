using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Queries
{
    public class AllEmployeesQuery
    {
        private readonly IQueryable<Employee> _employeesDb;

        public AllEmployeesQuery(IQueryable<Employee> employeesDb)
        {
            _employeesDb = employeesDb;
        }
        
        public async Task<IReadOnlyCollection<Employee>> Execute()
        {
            var employees = await _employeesDb
                .ToListAsync();

            return employees;
        }
    }
}