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
    public class GradeService : IGradeService
    {
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly IGradeRepository gradeRepository;
        private readonly ICourseRepository courseRepository;

        public GradeService(
            IEnrollmentRepository enrollmentRepository,
            IGradeRepository gradeRepository,
            ICourseRepository courseRepository)
        {
            this.enrollmentRepository = enrollmentRepository;
            this.gradeRepository = gradeRepository;
            this.courseRepository = courseRepository;
        }

        public async Task AddAsync(Grades grade)
        {
            await gradeRepository.CreateAsync(grade);
            await gradeRepository.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool gradeExists = await gradeRepository.ExistsAsync(g => g.GradeId == id);
            if (!gradeExists)
            {
                //TODO thow a custom exception
            }

            await gradeRepository.DeleteAsync(id);
            await gradeRepository.SaveAsync();
        }

        public async Task<IEnumerable<Grades>> GetAllAsync()
        {
            return (await gradeRepository.FindAsync()).ToList();
        }

        public async Task<Grades> GetFirstOrDefaultAsync(Expression<Func<Grades, bool>> filter)
        {
            return (await gradeRepository.FindAsync(filter)).FirstOrDefault();
        }

        //get all grades of a student based on his id
        public async Task<IEnumerable<StudentGrade>> GetGradesForStudentAsync(Guid studentId)
        {
            var enrollments = (await enrollmentRepository.FindAsync(e => e.StudentId == studentId)).ToList();
            var dictionary = new Dictionary<Enrollments, Courses>();
            var grades = await gradeRepository.GetGradesAsync(enrollments);
            foreach (var enrollment in enrollments)
            {
                dictionary.Add(enrollment, (await courseRepository.FindAsync(c => c.CourseId == enrollment.CourseId)).FirstOrDefault());
            }
            
            var studentGrades = new List<StudentGrade>();
            foreach(var item in grades)
            {
                // search the enrollment
                var enrollment = (await enrollmentRepository.FindAsync(e => e.EnrollmentId == item.EnrollmentId)).FirstOrDefault();
                // search the course
                var course = (await courseRepository.FindAsync(c => c.CourseId == enrollment.CourseId)).FirstOrDefault();
                // create the studentGrade object
                studentGrades.Add(new StudentGrade {GradeValue=item.Value, Date = item.Date, CourseTitle=course.CourseTitle });
            }
            return studentGrades;
        }

        public async Task UpdateAsync(Grades grade)
        {
            bool gradeExists = await gradeRepository.ExistsAsync(g => g.GradeId == grade.GradeId);
            if (!gradeExists)
            {
                //TODO thow a custom exception
            }

            await gradeRepository.UpdateAsync(grade);
            await gradeRepository.SaveAsync();
        }
    }
}
