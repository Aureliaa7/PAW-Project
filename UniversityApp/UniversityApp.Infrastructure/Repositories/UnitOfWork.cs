using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces;
using UniversityApp.Repositories;

namespace UniversityApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityAppContext dbContext;
        private IRepositoryBase<User> usersRepository;
        private IRepositoryBase<Student> studentsRepository;
        private IRepositoryBase<Course> coursesRepository;
        private IRepositoryBase<Enrollment> enrollmentsRepository;
        private IRepositoryBase<Secretary> secretariesRepository;
        private IRepositoryBase<Teacher> teachersRepository;
        private IRepositoryBase<TeachedCourse> teachedCoursesRepository;
        private IRepositoryBase<Grade> gradesRepository;


        public UnitOfWork(UniversityAppContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepositoryBase<User> UsersRepository
        {
            get
            {
                if (usersRepository == null)
                {
                    usersRepository = new RepositoryBase<User>(dbContext);
                }
                return usersRepository;
            }
        }

        public IRepositoryBase<Student> StudentsRepository
        {
            get
            {
                if (studentsRepository == null)
                {
                    studentsRepository = new RepositoryBase<Student>(dbContext);
                }
                return studentsRepository;
            }
        }

        public IRepositoryBase<Course> CoursesRepository
        {
            get
            {
                if (coursesRepository == null)
                {
                    coursesRepository = new RepositoryBase<Course>(dbContext);
                }
                return coursesRepository;
            }
        }

        public IRepositoryBase<Enrollment> EnrollmentsRepository
        {
            get
            {
                if (enrollmentsRepository == null)
                {
                    enrollmentsRepository = new RepositoryBase<Enrollment>(dbContext);
                }
                return enrollmentsRepository;
            }
        }

        public IRepositoryBase<Secretary> SecretariesRepository
        {
            get
            {
                if (secretariesRepository == null)
                {
                    secretariesRepository = new RepositoryBase<Secretary>(dbContext);
                }
                return secretariesRepository;
            }
        }

        public IRepositoryBase<Teacher> TeachersRepository
        {
            get
            {
                if (teachersRepository == null)
                {
                    teachersRepository = new RepositoryBase<Teacher>(dbContext);
                }
                return teachersRepository;
            }
        }

        public IRepositoryBase<TeachedCourse> TeachedCoursesRepository
        {
            get
            {
                if (teachedCoursesRepository == null)
                {
                    teachedCoursesRepository = new RepositoryBase<TeachedCourse>(dbContext);
                }
                return teachedCoursesRepository;
            }
        }

        public IRepositoryBase<Grade> GradesRepository
        {
            get
            {
                if (gradesRepository == null)
                {
                    gradesRepository = new RepositoryBase<Grade>(dbContext);
                }
                return gradesRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
