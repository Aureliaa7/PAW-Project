using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Models;

namespace UniversityApp.Interfaces.Repositories
{
    public interface IEnrollmentRepository : IRepositoryBase<Enrollments>
    {
        // get all the enrollments for a specific student
        IEnumerable<Enrollments> GetEnrollments(int studentId);
    }
}
