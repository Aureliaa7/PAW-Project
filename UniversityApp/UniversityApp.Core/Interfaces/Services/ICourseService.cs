using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ICourseService : ICrudOperationsService<Course>
    {
        // get all the students taking a course
        Task<IEnumerable<Student>> GetEnrolledStudents(Guid courseId);
    }
}
