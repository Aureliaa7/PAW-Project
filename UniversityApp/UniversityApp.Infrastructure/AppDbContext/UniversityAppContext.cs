using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Infrastructure.AppDbContext
{
    public partial class UniversityAppContext : IdentityDbContext<User>
    {
        public UniversityAppContext() { }

        public UniversityAppContext(DbContextOptions<UniversityAppContext> options) : base(options) { }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Secretary> Secretaries { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<TeachedCourse> TeachedCourses { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

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
