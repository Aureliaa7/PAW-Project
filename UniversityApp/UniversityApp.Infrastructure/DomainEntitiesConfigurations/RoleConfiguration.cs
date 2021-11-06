using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.DomainEntitiesConfigurations
{
    class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Secretary",
                    NormalizedName = "SECRETARY"
                },
                 new ApplicationRole
                 {
                     Id = Guid.NewGuid(),
                     Name = "Student",
                     NormalizedName = "STUDENT"
                 },
                  new ApplicationRole
                  {
                      Id = Guid.NewGuid(),
                      Name = "Teacher",
                      NormalizedName = "TEACHER"
                  }
                );
        }
    }
}
