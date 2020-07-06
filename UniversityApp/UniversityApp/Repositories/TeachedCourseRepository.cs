using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class TeachedCourseRepository : RepositoryBase<TeachedCourses>, ITeachedCourseRepository
    {
        public TeachedCourseRepository(UniversityAppContext context):base(context) { }
    }
}
