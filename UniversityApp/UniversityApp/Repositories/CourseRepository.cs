using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class CourseRepository : RepositoryBase<Courses>, ICourseRepository
    {
        public CourseRepository(UniversityAppContext context) : base(context) { }

    }
}
