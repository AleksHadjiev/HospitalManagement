using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HospitalManagement.Models
{
    public partial class Patient
    {
        public Guid Id { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
        [Required]
        public Guid DoctorId { get; set; }
        [MaxLength(64), Required]
        public string FullName { get; set; }
        public int? Age { get; set; }
        [Required]
        public long PersonalId { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool? Insurance { get; set; }

        public virtual Department Department { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
