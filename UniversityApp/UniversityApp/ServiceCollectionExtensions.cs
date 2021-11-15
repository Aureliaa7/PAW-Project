using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.DomainServices;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Infrastructure.Repositories;

namespace UniversityApp.Presentation
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISecretaryService, SecretaryService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ITeachedCourseService, TeachedCourseService>();
            services.AddScoped<IFindLoggedInUser, FindLoggedUser>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IImageService, ImageService>();
        }

        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UniversityAppContext>(options =>
              options.UseSqlServer(
                  configuration.GetConnectionString("MyConnectionString"),
                  b => b.MigrationsAssembly("UniversityApp.Infrastructure"))
            );
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, ApplicationRole>()
            .AddEntityFrameworkStores<UniversityAppContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
