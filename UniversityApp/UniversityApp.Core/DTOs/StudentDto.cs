using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DTOs
{
    public class StudentDto : UserDto
    {
        [Display(Name = "Study year")]
        public int StudyYear { get; set; }

        public string Section { get; set; }

        [Display(Name = "Group name")]
        public string GroupName { get; set; }
    }
}
