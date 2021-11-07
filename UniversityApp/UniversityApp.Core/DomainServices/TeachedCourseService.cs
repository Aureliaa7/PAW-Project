using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.DomainServices
{
    public class TeachedCourseService : ITeachedCourseService
    {
        private readonly IUnitOfWork unitOfWork;

        public TeachedCourseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AssignCourseAsync(CourseAssignmentViewModel model)
        {
            var course = (await unitOfWork.CoursesRepository.GetAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            var teacher = (await unitOfWork.TeachersRepository.GetAsync(t => String.Equals(t.Cnp, model.TeacherCnp))).FirstOrDefault();
            if(course != null && teacher != null)
            {
                var teachedCourse = new TeachedCourse() { TeacherId = teacher.Id, CourseId = course.Id };
                await unitOfWork.TeachedCoursesRepository.CreateAsync(teachedCourse);
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TeachedCourse>> GetAsync(Expression<Func<TeachedCourse, bool>> filter = null)
        {
            return (await unitOfWork.TeachedCoursesRepository.GetAsync(filter)).ToList();
        }

        public async Task<TeachedCourse> GetFirstOrDefaultAsync(Expression<Func<TeachedCourse, bool>> filter)
        {
            return (await unitOfWork.TeachedCoursesRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Course>> GetTeachedCoursesAsync(Guid teacherId)
        {
            var teachedCourses = (await unitOfWork.TeachedCoursesRepository.GetAsync(tc => tc.TeacherId == teacherId));
            var courseNames = new List<Course>();
            foreach(var tc in teachedCourses)
            {
                courseNames.Add((await unitOfWork.CoursesRepository.GetAsync(c => c.Id == tc.CourseId)).ToList().FirstOrDefault());
            }
            return courseNames;
        }

        public async Task UpdateAsync(TeachedCourse teachedCourse)
        {
            bool teachedCourseExists = await unitOfWork.TeachedCoursesRepository.ExistsAsync(tc => tc.Id == teachedCourse.Id);
            if (!teachedCourseExists)
            {
                throw new EntityNotFoundException($"The teached course with the id {teachedCourse.Id} was not found!");
            }

            await unitOfWork.TeachedCoursesRepository.UpdateAsync(teachedCourse);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
