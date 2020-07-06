using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;
using UniversityApp.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Services
{
    public class TeachedCourseService : ITeachedCourseService
    {
        public TeachedCourseService(UniversityAppContext context)
        {
            CourseRepository = new CourseRepository(context);
            TeacherRepository = new TeacherRepository(context);
            TeachedCourseRepository = new TeachedCourseRepository(context);
        }
        public ICourseRepository CourseRepository { get; }

        public ITeacherRepository TeacherRepository { get; }

        public ITeachedCourseRepository TeachedCourseRepository { get; }

        public void AssignCourse(CourseAssignmentViewModel model)
        {
            var course = CourseRepository.FindByCondition(c => String.Equals(c.CourseTitle, model.CourseTitle)).FirstOrDefault();
            var teacher = TeacherRepository.FindByCondition(t => String.Equals(t.Cnp, model.TeacherCnp)).FirstOrDefault();
            if(course != null && teacher != null)
            {
                var teachedCourse = new TeachedCourses() { TeacherId = teacher.TeacherId, CourseId = course.CourseId };
                TeachedCourseRepository.Create(teachedCourse);
            }
        }

        public IEnumerable<Courses> GetTeachedCourses(int teacherId)
        {
            var teachedCourses = TeachedCourseRepository.FindByCondition(tc => tc.TeacherId == teacherId);
            var courseNames = new List<Courses>();
            foreach(var tc in teachedCourses)
            {
                //courseNames.Append(CourseRepository.FindByCondition(c => c.CourseId == tc.CourseId).FirstOrDefault());
                courseNames.Add(CourseRepository.FindByCondition(c => c.CourseId == tc.CourseId).FirstOrDefault());
            }
            return courseNames;
        }
    }
}
