using System;
using System.Collections.Generic;

namespace StudentsProfileApplication.Models;

public partial class TeachingAssignment
{
    public int AssignmentId { get; set; }

    public int TeacherId { get; set; }

    public int CourseId { get; set; }

    public DateTime AssignmentDate { get; set; }

    public virtual Course Courses { get; set; } = null!;

    public virtual Teacher Teachers { get; set; } = null!;
}
