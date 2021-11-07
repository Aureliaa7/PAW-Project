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
        private IRepository<User> usersRepository;
        private IRepository<Student> studentsRepository;
        private IRepository<Course> coursesRepository;
        private IRepository<Enrollment> enrollmentsRepository;
        private IRepository<Secretary> secretariesRepository;
        private IRepository<Teacher> teachersRepository;
        private IRepository<TeachedCourse> teachedCoursesRepository;
        private IRepository<Grade> gradesRepository;


        public UnitOfWork(UniversityAppContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepository<User> UsersRepository
        {
            get
            {
                if (usersRepository == null)
                {
                    usersRepository = new Repository<User>(dbContext);
                }
                return usersRepository;
            }
        }

        public IRepository<Student> StudentsRepository
        {
            get
            {
                if (studentsRepository == null)
                {
                    studentsRepository = new Repository<Student>(dbContext);
                }
                return studentsRepository;
            }
        }

        public IRepository<Course> CoursesRepository
        {
            get
            {
                if (coursesRepository == null)
                {
                    coursesRepository = new Repository<Course>(dbContext);
                }
                return coursesRepository;
            }
        }

        public IRepository<Enrollment> EnrollmentsRepository
        {
            get
            {
                if (enrollmentsRepository == null)
                {
                    enrollmentsRepository = new Repository<Enrollment>(dbContext);
                }
                return enrollmentsRepository;
            }
        }

        public IRepository<Secretary> SecretariesRepository
        {
            get
            {
                if (secretariesRepository == null)
                {
                    secretariesRepository = new Repository<Secretary>(dbContext);
                }
                return secretariesRepository;
            }
        }

        public IRepository<Teacher> TeachersRepository
        {
            get
            {
                if (teachersRepository == null)
                {
                    teachersRepository = new Repository<Teacher>(dbContext);
                }
                return teachersRepository;
            }
        }

        public IRepository<TeachedCourse> TeachedCoursesRepository
        {
            get
            {
                if (teachedCoursesRepository == null)
                {
                    teachedCoursesRepository = new Repository<TeachedCourse>(dbContext);
                }
                return teachedCoursesRepository;
            }
        }

        public IRepository<Grade> GradesRepository
        {
            get
            {
                if (gradesRepository == null)
                {
                    gradesRepository = new Repository<Grade>(dbContext);
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
