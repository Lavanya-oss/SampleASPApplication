using StudentsProfileApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentsProfileApplication.ViewModels;

public partial class StudentViewModel
{
    public int StudentId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public string FirstName { get; set; } = null!;

    public decimal? Gpa { get; set; }

    public string LastName { get; set; } = null!;

    public Nullable<int> Age { get; set; } = null!;

    public bool HasScholarShip { get; set; }

    public string? CourseNames { get; set; }

    public Department SelectedDepartment { get; set; }

    public int DepartmentID { get; set; }

    public int AddressId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string FirtName { get; set; } = null!; // Non-nullable string
}
