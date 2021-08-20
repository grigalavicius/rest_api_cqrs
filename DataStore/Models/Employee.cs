using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = default!;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = default!;
        [Required]
        [Column(TypeName="date")]
        public DateTime BirthDate { get; set; }
        [Required]
        [Column(TypeName="date")]
        public DateTime EmploymentDate { get; set; }
        public int? BossId { get; set; }
        [Required]
        public string HomeAddress { get; set; } = default!;
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public Role Role { get; set; }
        [ForeignKey(nameof(BossId))]
        public virtual Employee Boss { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = default!;
    }
}