using System;
using System.Collections.Generic;

namespace Application.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public DateTime EmploymentDate { get; set; }
        
        public string HomeAddress { get; set; }
        
        public decimal Salary { get; set; }

        public string Role { get; set; }

        public EmployeeDto Boss { get; set; }
        public IReadOnlyCollection<EmployeeDto> Employees { get; set; }
    }
}