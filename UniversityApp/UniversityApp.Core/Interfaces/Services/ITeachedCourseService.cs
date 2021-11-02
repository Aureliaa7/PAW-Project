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
        Task<IEnumerable<Course>> GetTeachedCoursesAsync(Guid teacherId); // this will be used for secretary in order to see the teached courses of a teacher
       
        Task AssignCourseAsync(CourseAssignmentViewModel model);

        Task<IEnumerable<TeachedCourse>> GetAsync(Expression<Func<TeachedCourse, bool>> filter = null);

        Task UpdateAsync(TeachedCourse teachedCourse);
    }
}
