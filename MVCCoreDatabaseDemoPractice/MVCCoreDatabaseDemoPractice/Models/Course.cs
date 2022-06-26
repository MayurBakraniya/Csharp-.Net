using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MVCCoreDatabaseDemoPractice.Models
{
    public partial class Course
    {
        public Course()
        {
            Student = new HashSet<Student>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }
}
