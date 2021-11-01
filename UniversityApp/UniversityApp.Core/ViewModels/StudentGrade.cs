using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
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
