using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ITeachedCourseService
    {
        Task<IEnumerable<Courses>> GetTeachedCoursesAsync(Guid teacherId); // this will be used for secretary in order to see the teached courses of a teacher
       
        Task AssignCourseAsync(CourseAssignmentViewModel model);

        Task<IEnumerable<TeachedCourses>> GetAsync(Expression<Func<TeachedCourses, bool>> filter = null);

        Task UpdateAsync(TeachedCourses teachedCourse);
    }
}
