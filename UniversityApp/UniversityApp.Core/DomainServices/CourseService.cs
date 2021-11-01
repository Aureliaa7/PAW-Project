using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Core.DomainServices
{
    public class CourseService : ICourseService
    {
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly ICourseRepository courseRepository;

        public CourseService(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
        {
            this.enrollmentRepository = enrollmentRepository;
            this.courseRepository = courseRepository;
        }

        public async Task<Courses> AddAsync(Courses course)
        {
            bool courseExists = await courseRepository.ExistsAsync(c => c.CourseTitle == course.CourseTitle);

            if (courseExists)
            {
                //TODO create and throw a custom exception
                throw new Exception("A course with the same title already exists!");
            }

            var addedCourse = await courseRepository.CreateAsync(course);
            await courseRepository.SaveAsync();

            return addedCourse;
        }

        public async Task DeleteAsync(Guid id)
        {
            bool courseExists = await courseRepository.ExistsAsync(c => c.CourseId == id);

            if (!courseExists)
            {
                //TODO create and throw a custom exception
                throw new Exception($"The course with the id {id} was not found!");
            }

            await courseRepository.DeleteAsync(id);
            await courseRepository.SaveAsync();
        }

        public async Task<Courses> UpdateAsync(Courses course)
        {
            bool courseExists = await courseRepository.ExistsAsync(c => c.CourseId == course.CourseId);

            if (!courseExists)
            {
                //TODO create and throw a custom exception
                throw new Exception($"The course with the id {course.CourseId} was not found!");
            }

            var updatedCourse = await courseRepository.UpdateAsync(course);
            await courseRepository.SaveAsync();

            return updatedCourse;
        }

        public async Task<IEnumerable<Courses>> GetAsync(Expression<Func<Courses, bool>> filter = null)
        {
            return (await courseRepository.FindAsync(filter)).ToList();
        }

        // returns all the students taking a certain course
        public async Task<IEnumerable<Students>> GetEnrolledStudents(Guid courseId)
        {
            var enrollments = await enrollmentRepository.FindAsync(enrollment => enrollment.CourseId == courseId);
            var enrolledStudents = enrollments.Select(enrollment => enrollment.Student).ToList();
            return enrolledStudents;
        }

        public async Task<Courses> GetFirstOrDefaultAsync(Expression<Func<Courses, bool>> filter)
        {
            return await (await courseRepository.FindAsync(filter)).FirstOrDefaultAsync();
        }
    }
}
