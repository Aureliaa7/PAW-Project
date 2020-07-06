using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Models;

namespace UniversityApp.Interfaces.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Courses>
    {
        //IEnumerable<Courses> GetTookCourses(int studentId);
    }
}
