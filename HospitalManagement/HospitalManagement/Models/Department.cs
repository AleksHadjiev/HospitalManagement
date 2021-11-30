using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HospitalManagement.Models
{
    public partial class Department
    {
        public Department()
        {
            Patients = new HashSet<Patient>();
        }

        public Guid Id { get; set; }
        [MaxLength(128), Required]
        public string DepartmentName { get; set; }
        public int? Capacity { get; set; }
        public long? DepartmentNumber { get; set; }
        public bool? HeadNurse { get; set; }
        public int? NursesCount { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
