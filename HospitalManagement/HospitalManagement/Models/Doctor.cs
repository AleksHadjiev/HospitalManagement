using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HospitalManagement.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Patients = new HashSet<Patient>();
        }

        public Guid Id { get; set; }
        [MaxLength(64), Required]
        public string FullName { get; set; }
        public int? Age { get; set; }
        [Required]
        public long PersonalId { get; set; }
        [MaxLength(128), Required]
        public string Specialization { get; set; }
        [Required]
        public DateTime ExperienceStartDate { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
