using System;
using System.Collections.Generic;
using System.Linq;
using Application.Models;
using DataStore.Models;
using RestApiTask.Commands;

namespace Api.Tests
{
    internal class TestsMockData
    {
        internal static IReadOnlyCollection<Employee> MockEmployees()
        {
            return new List<Employee>()
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    BirthDate = new DateTime(1990, 01, 01),
                    EmploymentDate = new DateTime(2021, 08, 18),
                    BossId = null,
                    HomeAddress = "Pakalnutės g. 1 - 12, Vilnius",
                    Salary = 3000,
                    Role = Role.Ceo
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Laura",
                    LastName = "Jannsen",
                    BirthDate = new DateTime(1985, 5, 16),
                    EmploymentDate = new DateTime(2020, 01, 10),
                    BossId = 1,
                    HomeAddress = "Lapų g. 1 - 1, Kaunas",
                    Salary = 900,
                    Role = Role.Administrator
                },
                new Employee
                {
                    Id = 3,
                    FirstName = "Will",
                    LastName = "Bill",
                    BirthDate = new DateTime(1982, 2, 19),
                    EmploymentDate = new DateTime(2010, 07, 4),
                    BossId = 1,
                    HomeAddress = "Smėlio g. 5 - 34, Vilnius",
                    Salary = 1200,
                    Role = Role.Waiter
                },
                new Employee
                {
                    Id = 4,
                    FirstName = "Piotr",
                    LastName = "Aleksandrovič",
                    BirthDate = new DateTime(1986, 12, 24),
                    EmploymentDate = new DateTime(2021, 07, 15),
                    BossId = 1,
                    HomeAddress = "Spalvų g. 8 - 31, Vilnius",
                    Salary = 600,
                    Role = Role.Seller
                },
                new Employee
                {
                    Id = 5,
                    FirstName = "Marija",
                    LastName = "Žamė",
                    BirthDate = new DateTime(1977, 2, 16),
                    EmploymentDate = new DateTime(2019, 02, 15),
                    BossId = 1,
                    HomeAddress = "Kažkokia g. 13 - 31, Vilnius",
                    Salary = 666,
                    Role = Role.Administrator
                },
                new Employee
                {
                    Id = 6,
                    FirstName = "Laima",
                    LastName = "Palaima",
                    BirthDate = new DateTime(1966, 01, 13),
                    EmploymentDate = new DateTime(2019, 07, 28),
                    BossId = 2,
                    HomeAddress = "Bespalvė g. 6 - 111, Vilnius",
                    Salary = 600,
                    Role = Role.Waiter
                },
                new Employee
                {
                    Id = 7,
                    FirstName = "Rimas",
                    LastName = "Nerimas",
                    BirthDate = new DateTime(1987, 05, 01),
                    EmploymentDate = new DateTime(2016, 08, 15),
                    BossId = 1,
                    HomeAddress = "Kalafijorų g. 9 - 99, Vilnius",
                    Salary = 789,
                    Role = Role.Administrator
                },
                new Employee
                {
                    Id = 8,
                    FirstName = "Arthur",
                    LastName = "Samsun",
                    BirthDate = new DateTime(1972, 6, 1),
                    EmploymentDate = new DateTime(2015, 10, 30),
                    BossId = 1,
                    HomeAddress = "Vasaros g. 6 - 3, Vilnius",
                    Salary = 2000,
                    Role = Role.Administrator
                },
                new Employee
                {
                    Id = 9,
                    FirstName = "Martin",
                    LastName = "Pamartin",
                    BirthDate = new DateTime(1999, 11, 01),
                    EmploymentDate = new DateTime(2021, 08, 01),
                    BossId = 3,
                    HomeAddress = "Marių g. 9 - 43, Vilnius",
                    Salary = 564,
                    Role = Role.Waiter
                },
                new Employee
                {
                    Id = 10,
                    FirstName = "Inga",
                    LastName = "Laiminga",
                    BirthDate = new DateTime(1999, 12, 25),
                    EmploymentDate = new DateTime(2020, 07, 06),
                    BossId = 8,
                    HomeAddress = "Palaimos g. 9 - 11, Vilnius",
                    Salary = 720,
                    Role = Role.Waiter
                }
            };
        }

        internal static IEnumerable<EmployeeDto> ExpectedEmployeesResult(IList<int> ids)
        {
            return MockEmployeeDtos().Where(x => ids.Contains(x.Id)).ToList();
        }
        
        internal static IEnumerable<EmployeeDto> ExpectedEmployeesByNameAndBirthdateRangeResult(string name, DateTime from, DateTime to)
        {
            var nameInLower = name.ToLower();
            return MockEmployeeDtos()
                .Where(x => x.FirstName.ToLower().Contains(nameInLower) || x.LastName.ToLower().Contains(nameInLower))
                .Where(x => x.BirthDate >= from && x.BirthDate <= to)
                .ToList();
        }
        
        internal static IEnumerable<EmployeeDto> ExpectedEmployeesByBossIdResult(int? bossId)
        {
            return MockEmployeeDtos().Where(x => x.BossId == bossId).ToList();
        }
        
        internal static IEnumerable<EmployeeDto> MockEmployeeDtos()
        {
            return MockEmployees().Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                EmploymentDate = x.EmploymentDate,
                HomeAddress = x.HomeAddress,
                Salary = x.Salary,
                Role = x.Role.ToString(),
                BossId = x.BossId
            });
        }

        public static EmployeesStatisticsByRoleDto ExpectedEmployeesStatisticsByRoleResult(Role role)
        {
            return MockEmployees()
                .Where(x => x.Role == role)
                .GroupBy(x => x.Role)
                .Select(x => new EmployeesStatisticsByRoleDto
                {
                    Count = x.Count(),
                    AverageSalary = x.Average(y => y.Salary)
                })
                .FirstOrDefault();
        }

        public static CreateEmployeeDtoCmd MockCreateEmployeeDtoCmd()
        {
            return new CreateEmployeeDtoCmd()
            {
                FirstName = "Vardenis",
                LastName = "Pavardenis",
                BirthDate = new DateTime(1988, 4, 4),
                EmploymentDate = new DateTime(2021, 8, 1),
                BossId = 2,
                HomeAddress = "Home Address street 15 - 5",
                Salary = (decimal)666.6,
                Role = Role.Seller
            };
        }
        
        public static UpdateEmployeeDtoCmd MockUpdateEmployeeDtoCmd()
        {
            return new UpdateEmployeeDtoCmd()
            {
                FirstName = "Anetta",
                LastName = "Andersen",
                BirthDate = new DateTime(1980, 6, 1),
                EmploymentDate = new DateTime(2015, 3, 15),
                BossId = 1,
                HomeAddress = "Home Address street 15 - 5",
                Salary = (decimal)1111.1,
                Role = Role.Administrator
            };
        }
        
        public static UpdateEmployeeSalaryDtoCmd MockUpdateEmployeesSalaryDtoCmd(decimal salary)
        {
            return new UpdateEmployeeSalaryDtoCmd()
            {
                Salary = salary,
            };
        }
    }
}