using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class EnrollmentRepository : RepositoryBase<Enrollments>, IEnrollmentRepository
    {
        public EnrollmentRepository(UniversityAppContext context) : base(context) { }

        public IEnumerable<Enrollments> GetEnrollments(int studentId)
        {
            var enrollments = FindByCondition(enrollment => enrollment.StudentId == studentId).Select(enrollment => enrollment).ToList();
            return enrollments;
        }
    }
}
