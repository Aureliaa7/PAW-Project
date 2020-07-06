using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UniversityApp.Models
{
    public partial class UniversityAppContext : IdentityDbContext<Users>
    {
        public UniversityAppContext()
        {
        }

        public UniversityAppContext(DbContextOptions<UniversityAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Enrollments> Enrollments { get; set; }
        public virtual DbSet<Grades> Grades { get; set; }
        public virtual DbSet<Secretaries> Secretaries { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<TeachedCourses> TeachedCourses { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Secretary",
                    NormalizedName = "SECRETARY"
                },
                 new IdentityRole
                 {
                     Id = "2",
                     Name = "Student",
                     NormalizedName = "STUDENT"
                 },
                  new IdentityRole
                  {
                      Id = "3",
                      Name = "Teacher",
                      NormalizedName = "TEACHER"
                  }
                );
        }
    }
}
