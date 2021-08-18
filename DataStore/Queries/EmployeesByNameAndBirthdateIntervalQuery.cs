using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Queries
{
    public class EmployeesByNameAndBirthdateIntervalQuery
    {
        private readonly IQueryable<Employee> _employeesDb;

        public EmployeesByNameAndBirthdateIntervalQuery(IQueryable<Employee> employeesDb)
        {
            _employeesDb = employeesDb;
        }
        
        public async Task<IReadOnlyCollection<Employee>> Execute(string name, DateTime from, DateTime to)
        {
            var nameInLower = name.ToLower();
            var employees = await _employeesDb
                .Where(x => x.FirstName.ToLower().Contains(nameInLower)  || x.LastName.ToLower().Contains(nameInLower))
                .Where(x => x.BirthDate >= from && x.BirthDate <= to)
                .ToListAsync();

            return employees;
        }
    }
}