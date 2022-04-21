﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteExample.Entities
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        [ForeignKey("CourseId")]    
        public int CourseId { get; set; }
        public Decimal? Grade { get; set; }
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<EnrollmentHistory> EnrollmentHistory { get; set; }
    }
}
