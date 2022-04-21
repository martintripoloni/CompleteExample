﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompleteExample.Entities
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
