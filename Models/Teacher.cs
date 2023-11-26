using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsProfileApplication.Models;

public partial class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
}
