using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApp.Core;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Cnp)
               .HasMaxLength(Constants.CnpLength)
               .IsRequired();

            builder.Property(x => x.FirstName)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(x => x.LastName)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(x => x.Email)
               .HasMaxLength(60)
               .IsRequired();
        }
    }
}
