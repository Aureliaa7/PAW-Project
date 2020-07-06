using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces.Services
{
    public interface ITeachedCourseService
    {
        ITeachedCourseRepository TeachedCourseRepository { get; }
        ICourseRepository CourseRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        IEnumerable<Courses> GetTeachedCourses(int teacherId); // this will be used for secretary in order to see the teached courses of a teacher
        void AssignCourse(CourseAssignmentViewModel model);
    }
}
