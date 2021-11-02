using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Interfaces;

namespace UniversityApp.Core.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepositoryBase<User> UsersRepository { get; }

        IRepositoryBase<Student> StudentsRepository { get; }

        IRepositoryBase<Course> CoursesRepository { get; }

        IRepositoryBase<Enrollment> EnrollmentsRepository { get; }

        IRepositoryBase<Secretary> SecretariesRepository { get; }

        IRepositoryBase<Teacher> TeachersRepository { get; }

        IRepositoryBase<TeachedCourse> TeachedCoursesRepository { get; }

        IRepositoryBase<Grade> GradesRepository { get; }

        Task SaveChangesAsync();
    }
}
