using AutoMapper;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Infrastructure
{
    public class MappingConfigurations : Profile
    {
        public MappingConfigurations()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            CreateMap<Course, CourseDto>().ReverseMap();

            CreateMap<Grade, GradeDto>().ReverseMap();

            CreateMap<SecretaryRegistrationDto, Secretary>().ForMember(x => x.Image, opt => opt.Ignore());
            CreateMap<Secretary, SecretaryDto>().ReverseMap();
            CreateMap<EditSecretaryDto, Secretary>().ReverseMap();

            CreateMap<User, UserDto>();
            CreateMap<Student, StudentDto>();
            CreateMap<StudentRegistrationViewModel, Student>().ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<TeacherRegistrationViewModel, Teacher>().ForMember(x => x.Image, opt => opt.Ignore());
            CreateMap<Teacher, TeacherDto>();
        }
    }
}
