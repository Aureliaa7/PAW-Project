using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string CourseTitle { get; set; }

        [Required]
        [Display(Name = "Number of credits")]
        [Range(Constants.MinNumberOfCredits, Constants.MaxNumberOfCredits)]
        public int NoCredits { get; set; }

        [Required]
        [Range(Constants.MinStudyYear, Constants.MaxStudyYear)]
        public int Year { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "The semester can be either 1 or 2")]
        public int Semester { get; set; }
    }
}
