using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork unitOfWork;
        public CourseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Course> AddAsync(Course course)
        {
            bool courseExists = await unitOfWork.CoursesRepository.ExistsAsync(c => c.CourseTitle == course.CourseTitle);

            if (courseExists)
            {
                throw new DuplicatedEntityException("A course with the same title already exists!");
            }

            var addedCourse = await unitOfWork.CoursesRepository.CreateAsync(course);
            await unitOfWork.SaveChangesAsync(); ;

            return addedCourse;
        }

        public async Task DeleteAsync(Guid id)
        {
            await CheckIfCourseExistsAsync(id);

            await unitOfWork.CoursesRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            await CheckIfCourseExistsAsync(course.Id);

            var updatedCourse = await unitOfWork.CoursesRepository.UpdateAsync(course);
            await unitOfWork.SaveChangesAsync();

            return updatedCourse;
        }

        public async Task<IEnumerable<Course>> GetAsync(Expression<Func<Course, bool>> filter = null)
        {
            return (await unitOfWork.CoursesRepository.FindAsync(filter)).ToList();
        }

        // returns all the students taking a certain course
        public async Task<IEnumerable<Student>> GetEnrolledStudents(Guid courseId)
        {
            await CheckIfCourseExistsAsync(courseId);
            var enrollments = await unitOfWork.EnrollmentsRepository.FindAsync(enrollment => enrollment.CourseId == courseId);
            var enrolledStudents = enrollments.Select(enrollment => enrollment.Student).ToList();
            return enrolledStudents;
        }

        public async Task<Course> GetFirstOrDefaultAsync(Expression<Func<Course, bool>> filter)
        {
            return (await unitOfWork.CoursesRepository.FindAsync(filter)).FirstOrDefault();
        }

        private async Task CheckIfCourseExistsAsync(Guid id)
        {
            bool courseExists = await unitOfWork.CoursesRepository.ExistsAsync(c => c.Id == id);

            if (!courseExists)
            {
                throw new EntityNotFoundException($"The course with the id {id} was not found!");
            }
        }
    }
}
