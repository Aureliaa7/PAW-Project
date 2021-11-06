using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {

            builder.Property(x => x.CourseId)
                .IsRequired();

            builder.Property(x => x.EnrollmentId)
                .IsRequired();

            builder.Property(x => x.StudentId)
                .IsRequired();
        }
    }
}
