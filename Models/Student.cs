using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsProfileApplication.Models;

public partial class Student
{
    public int StudentId { get; set; }

    [Required]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    public DateTime? EnrollmentDate { get; set; }

    [Required(ErrorMessage = "Please enter name")]
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal? Gpa { get; set; }

    public int? Age { get; set; }

    public bool HasScholarShip { get; set; }

    public string? Password { get; set; }
    
    [ForeignKey("Departments")]
    public int DepartmentID { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    public virtual Department Departments { get; set; }
    public virtual List<Address>? Address { get; set; }
}
