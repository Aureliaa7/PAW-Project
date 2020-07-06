using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;

namespace UniversityApp.Interfaces
{
    public  interface ICourseService
    {
        IEnrollmentRepository EnrollmentRepository { get; }
        ICourseRepository CourseRepository { get; }

        // get all the students taking a course
        IEnumerable<Students> GetEnrolledStudents(int courseId);

    }
}
