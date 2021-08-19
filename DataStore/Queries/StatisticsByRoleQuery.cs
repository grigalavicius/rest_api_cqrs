using System.Linq;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Queries
{
    public class StatisticsByRoleQuery
    {
        private readonly IQueryable<Employee> _employeesDb;

        public StatisticsByRoleQuery(IQueryable<Employee> employeesDb)
        {
            _employeesDb = employeesDb;
        }

        public async Task<EmployeesStatisticsByRole> Execute(Role role)
        {
            var employeesCountAndAvgSalaryByRole = await _employeesDb
                .Where(x => x.Role == role)
                .GroupBy(x => x.Role)
                .Select(x => new EmployeesStatisticsByRole
                {
                    Count = x.Count(),
                    AverageSalary = x.Average(y => y.Salary)
                })
                .FirstOrDefaultAsync();

            return employeesCountAndAvgSalaryByRole;
        }
    }
}