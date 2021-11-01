using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
{
    public class DeleteStudentViewModel
    {
        [Required]
        [Display(Name = "Study year")]
        public int StudyYear { get; set; }
        [Required]
        [Display(Name = "Section name")]
        public string SectionName { get; set; }
        [Required]
        public string Cnp { get; set; }
        [Display(Name = "Student name")]
        public string StudentName { get; set; }
    }
}
