using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteExample.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        [ForeignKey("InstructorId")]
        public int InstructorId { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
