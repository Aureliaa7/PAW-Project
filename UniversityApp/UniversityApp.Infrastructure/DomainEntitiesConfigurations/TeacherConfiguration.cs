using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(x => x.Degree)
               .HasMaxLength(50)
               .IsRequired();
        }
    }
}
