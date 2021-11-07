using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Interfaces;

namespace UniversityApp.Core.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<User> UsersRepository { get; }

        IRepository<Student> StudentsRepository { get; }

        IRepository<Course> CoursesRepository { get; }

        IRepository<Enrollment> EnrollmentsRepository { get; }

        IRepository<Secretary> SecretariesRepository { get; }

        IRepository<Teacher> TeachersRepository { get; }

        IRepository<TeachedCourse> TeachedCoursesRepository { get; }

        IRepository<Grade> GradesRepository { get; }

        Task SaveChangesAsync();
    }
}
