using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsProfileApplication.Models
{
    public class StudentCourses
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("studentId")]
        public int studentId {  get; set; }
        public int coursesId { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
