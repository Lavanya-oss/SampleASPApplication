using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace StudentsProfileApplication.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        public string? Name { get; set; }

        public decimal CollegeFee { get; set; }

        public DateTime StartDate { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

    }
}
