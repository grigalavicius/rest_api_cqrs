using System;
using DataStore.Models;

namespace RestApiTask.Commands
{
    public class CreateEmployeeDtoCmd
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int? BossId { get; set; }
        public string HomeAddress { get; set; }
        public decimal Salary { get; set; }
        public Role Role { get; set; }
    }
}