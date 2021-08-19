using System;
using System.Linq;
using DataStore.Models;

namespace DataStore
{
    public class DataStoreInitializer
    {
        public static void Init(IEmployeesContext context)
        {
            RemovePreviouslyAddedEmployees(context);
            AddEmployees(context);
            context.SaveChanges();
        }

        private static void AddEmployees(IEmployeesContext context)
        {
            var item1 = new Employee
            {
                Id = int.MaxValue - 10,
                FirstName = "John",
                LastName = "Smith",
                BirthDate = new DateTime(1990, 01, 01),
                EmploymentDate = new DateTime(2021, 08, 18),
                BossId = null,
                HomeAddress = "Pakalnutės g. 1 - 12, Vilnius",
                Salary = 3000,
                Role = context.EmployeeWithCeoRoleExist().Result ? Role.Administrator : Role.Ceo
            };

            var item2 = new Employee
            {
                Id = int.MaxValue - 9,
                FirstName = "Laura",
                LastName = "Jannsen",
                BirthDate = new DateTime(1985, 5, 16),
                EmploymentDate = new DateTime(2020, 01, 10),
                BossId = int.MaxValue - 10,
                HomeAddress = "Lapų g. 1 - 1, Kaunas",
                Salary = 900,
                Role = Role.Administrator
            };

            var item3 = new Employee
            {
                Id = int.MaxValue - 8,
                FirstName = "Will",
                LastName = "Bill",
                BirthDate = new DateTime(1982, 2, 19),
                EmploymentDate = new DateTime(2010, 07, 4),
                BossId = int.MaxValue - 10,
                HomeAddress = "Smėlio g. 5 - 34, Vilnius",
                Salary = 1200,
                Role = Role.Waiter
            };

            var item4 = new Employee
            {
                Id = int.MaxValue - 7,
                FirstName = "Piotr",
                LastName = "Aleksandrovič",
                BirthDate = new DateTime(1986, 12, 24),
                EmploymentDate = new DateTime(2021, 07, 15),
                BossId = int.MaxValue - 10,
                HomeAddress = "Spalvų g. 8 - 31, Vilnius",
                Salary = 600,
                Role = Role.Seller
            };

            var item5 = new Employee
            {
                Id = int.MaxValue - 6,
                FirstName = "Marija",
                LastName = "Žamė",
                BirthDate = new DateTime(1977, 2, 16),
                EmploymentDate = new DateTime(2019, 02, 15),
                BossId = int.MaxValue - 10,
                HomeAddress = "Kažkokia g. 13 - 31, Vilnius",
                Salary = 666,
                Role = Role.Administrator
            };

            var item6 = new Employee
            {
                Id = int.MaxValue - 5,
                FirstName = "Laima",
                LastName = "Palaima",
                BirthDate = new DateTime(1966, 01, 13),
                EmploymentDate = new DateTime(2019, 07, 28),
                BossId = int.MaxValue - 9,
                HomeAddress = "Bespalvė g. 6 - 111, Vilnius",
                Salary = 600,
                Role = Role.Waiter
            };

            var item7 = new Employee
            {
                Id = int.MaxValue - 4,
                FirstName = "Rimas",
                LastName = "Nerimas",
                BirthDate = new DateTime(1987, 05, 01),
                EmploymentDate = new DateTime(2016, 08, 15),
                BossId = int.MaxValue - 10,
                HomeAddress = "Kalafijorų g. 9 - 99, Vilnius",
                Salary = 789,
                Role = Role.Administrator
            };

            var item8 = new Employee
            {
                Id = int.MaxValue - 3,
                FirstName = "Arthur",
                LastName = "Samsun",
                BirthDate = new DateTime(1972, 6, 1),
                EmploymentDate = new DateTime(2015, 10, 30),
                BossId = int.MaxValue - 10,
                HomeAddress = "Vasaros g. 6 - 3, Vilnius",
                Salary = 2000,
                Role = Role.Administrator
            };

            var item9 = new Employee
            {
                Id = int.MaxValue - 2,
                FirstName = "Martin",
                LastName = "Pamartin",
                BirthDate = new DateTime(1999, 11, 01),
                EmploymentDate = new DateTime(2021, 08, 01),
                BossId = int.MaxValue - 6,
                HomeAddress = "Marių g. 9 - 43, Vilnius",
                Salary = 564,
                Role = Role.Waiter
            };

            var item10 = new Employee
            {
                Id = int.MaxValue - 1,
                FirstName = "Inga",
                LastName = "Laiminga",
                BirthDate = new DateTime(1999, 12, 25),
                EmploymentDate = new DateTime(2020, 07, 06),
                BossId = int.MaxValue - 4,
                HomeAddress = "Palaimos g. 9 - 11, Vilnius",
                Salary = 720,
                Role = Role.Waiter
            };

            context.Employees.Add(item1);
            context.Employees.Add(item2);
            context.Employees.Add(item3);
            context.Employees.Add(item4);
            context.Employees.Add(item5);
            context.Employees.Add(item6);
            context.Employees.Add(item7);
            context.Employees.Add(item8);
            context.Employees.Add(item9);
            context.Employees.Add(item10);
        }

        private static void RemovePreviouslyAddedEmployees(IEmployeesContext context)
        {
            var employees = context.Employees.Where(x => new[]
            {
                int.MaxValue - 1, int.MaxValue - 2, int.MaxValue - 3, int.MaxValue - 4, int.MaxValue - 5, int.MaxValue - 6, int.MaxValue - 7, int.MaxValue - 8,
                int.MaxValue - 9, int.MaxValue - 10
            }.Contains(x.Id));
            context.Employees.RemoveRange(employees);
        }
    }
}