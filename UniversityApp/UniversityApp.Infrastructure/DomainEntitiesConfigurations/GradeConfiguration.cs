using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        { 
            builder.Property(x => x.EnrollmentId)
                .IsRequired();
        }
    }
}
