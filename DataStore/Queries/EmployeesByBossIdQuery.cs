using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Queries
{
    public class EmployeesByBossIdQuery
    {
        private readonly IQueryable<Employee> _employeesDb;

        public EmployeesByBossIdQuery(IQueryable<Employee> employeesDb)
        {
            _employeesDb = employeesDb;
        }

        public async Task<IReadOnlyCollection<Employee>> Execute(int? bossId)
        {
            var employees = await _employeesDb
                .Include(x => x.Boss)
                .Include(x => x.Employees)
                .Where(x => x.BossId == bossId)
                .ToListAsync();

            return employees;
        }
    }
}