using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Core.DomainServices
{
    public class TeachedCourseService : ITeachedCourseService
    {
        private readonly ICourseRepository courseRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ITeachedCourseRepository teachedCourseRepository;
        public TeachedCourseService(
            ICourseRepository courseRepository,
            ITeacherRepository teacherRepository,
            ITeachedCourseRepository teachedCourseRepository)
        {
            this.courseRepository = courseRepository;
            this.teacherRepository = teacherRepository;
            this.teachedCourseRepository = teachedCourseRepository;
        }

        public async Task AssignCourseAsync(CourseAssignmentViewModel model)
        {
            var course = (await courseRepository.FindAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            var teacher = (await teacherRepository.FindAsync(t => String.Equals(t.Cnp, model.TeacherCnp))).FirstOrDefault();
            if(course != null && teacher != null)
            {
                var teachedCourse = new TeachedCourses() { TeacherId = teacher.TeacherId, CourseId = course.CourseId };
                await teachedCourseRepository.CreateAsync(teachedCourse);
                await teachedCourseRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<TeachedCourses>> GetAsync(Expression<Func<TeachedCourses, bool>> filter = null)
        {
            return (await teachedCourseRepository.FindAsync(filter)).ToList();
        }

        public async Task<IEnumerable<Courses>> GetTeachedCoursesAsync(Guid teacherId)
        {
            var teachedCourses = (await teachedCourseRepository.FindAsync(tc => tc.TeacherId == teacherId));
            var courseNames = new List<Courses>();
            foreach(var tc in teachedCourses)
            {
                //courseNames.Append(CourseRepository.FindByCondition(c => c.CourseId == tc.CourseId).FirstOrDefault());
                courseNames.Add((await courseRepository.FindAsync(c => c.CourseId == tc.CourseId)).ToList().FirstOrDefault());
            }
            return courseNames;
        }

        public async Task UpdateAsync(TeachedCourses teachedCourse)
        {
            bool teachedCourseExists = await teachedCourseRepository.ExistsAsync(tc => tc.Id == teachedCourse.Id);
            if (!teachedCourseExists)
            {
                //TODO throw an exception
            }

            await teachedCourseRepository.UpdateAsync(teachedCourse);
            await teachedCourseRepository.SaveAsync();
        }
    }
}
