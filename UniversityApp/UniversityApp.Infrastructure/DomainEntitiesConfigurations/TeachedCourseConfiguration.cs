using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class TeachedCourseConfiguration : IEntityTypeConfiguration<TeachedCourse>
    {
        public void Configure(EntityTypeBuilder<TeachedCourse> builder)
        {
            builder.Property(x => x.CourseId)
               .IsRequired();

            builder.Property(x => x.TeacherId)
               .IsRequired();
        }
    }
}
