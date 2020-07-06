using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityApp.ViewModels
{
    public class StudentGrade
    {
        [Display(Name = "Grade")]
        public int? GradeValue { get; set; }
        public DateTime? Date { get; set; }
        [Display(Name = "Course")]
        public string CourseTitle { get; set; }
    }
}
