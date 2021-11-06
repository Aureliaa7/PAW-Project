using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.DomainEntitiesConfigurations;

namespace UniversityApp.Infrastructure.AppDbContext
{
    public partial class UniversityAppContext : IdentityDbContext<User, ApplicationRole, Guid>
    {
        public UniversityAppContext() { }

        public UniversityAppContext(DbContextOptions<UniversityAppContext> options) : base(options) { }

        public virtual DbSet<User> ApplicationUsers { get; set; }
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

            new CourseConfiguration().Configure(modelBuilder.Entity<Course>());
            new EnrollmentConfiguration().Configure(modelBuilder.Entity<Enrollment>());
            new GradeConfiguration().Configure(modelBuilder.Entity<Grade>());
            new RoleConfiguration().Configure(modelBuilder.Entity<ApplicationRole>());
            new StudentConfiguration().Configure(modelBuilder.Entity<Student>());
            new TeachedCourseConfiguration().Configure(modelBuilder.Entity<TeachedCourse>());
            new TeacherConfiguration().Configure(modelBuilder.Entity<Teacher>());
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
        }
    }
}
