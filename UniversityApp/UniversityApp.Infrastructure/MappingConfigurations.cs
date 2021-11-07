using AutoMapper;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DTOs;

namespace UniversityApp.Infrastructure
{
    public class MappingConfigurations : Profile
    {
        public MappingConfigurations()
        {
            CreateMap<Course, CourseDto>().ReverseMap();

            CreateMap<Grade, GradeDto>().ReverseMap();

            CreateMap<SecretaryRegistrationDto, Secretary>().ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<Secretary, SecretaryDto>().ReverseMap();

            CreateMap<EditSecretaryDto, Secretary>().ReverseMap();
        }
    }
}
