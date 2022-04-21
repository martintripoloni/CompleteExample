using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteExample.Entities
{
    public class EnrollmentHistory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("EnrollmentId")]
        public int EnrollmentId { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        public Decimal? Grade { get; set; }
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
        public virtual Enrollment Enrollment { get; set; }
    }
}
