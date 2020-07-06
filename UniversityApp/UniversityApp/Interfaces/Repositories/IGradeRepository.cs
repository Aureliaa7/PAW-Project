using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Models;

namespace UniversityApp.Interfaces
{
    public interface IGradeRepository : IRepositoryBase<Grades>
    {
        // get grades based on the enrollments
        IEnumerable<Grades> GetGrades(IEnumerable<Enrollments> enrollments);
    }
}
