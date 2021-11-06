using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(x => x.CourseTitle)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.NoCredits)
                .IsRequired();

            builder.Property(x => x.Year)
                .IsRequired();

            builder.Property(x => x.Semester)
                .IsRequired();
        }
    }
}
