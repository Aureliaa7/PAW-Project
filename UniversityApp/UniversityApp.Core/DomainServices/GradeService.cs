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
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork unitOfWork;

        public GradeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Grade> AddAsync(Grade grade)
        {
            var savedGrade = await unitOfWork.GradesRepository.CreateAsync(grade);
            await unitOfWork.SaveChangesAsync();

            return savedGrade;
        }

        public async Task DeleteAsync(Guid id)
        {
            bool gradeExists = await unitOfWork.GradesRepository.ExistsAsync(g => g.GradeId == id);
            if (!gradeExists)
            {
                throw new EntityNotFoundException($"The grade with the id {id} was not found!");
            }

            await unitOfWork.GradesRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Grade>> GetAllAsync(Expression<Func<Grade, bool>> filter = null)
        {
            return (await unitOfWork.GradesRepository.GetAsync(filter)).ToList();
        }

        public async Task<Grade> GetFirstOrDefaultAsync(Expression<Func<Grade, bool>> filter)
        {
            return (await unitOfWork.GradesRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<StudentGrade>> GetGradesForStudentAsync(Guid studentId)
        {
            var enrollments = (await unitOfWork.EnrollmentsRepository.GetAsync(e => e.StudentId == studentId)).ToList();
            var dictionary = new Dictionary<Enrollment, Course>();

            var grades = await GetGradesAsync(enrollments);
            foreach (var enrollment in enrollments)
            {
                dictionary.Add(enrollment, (await unitOfWork.CoursesRepository.GetAsync(c => c.Id == enrollment.CourseId)).FirstOrDefault());
            }
            
            var studentGrades = new List<StudentGrade>();
            foreach(var item in grades)
            {
                var enrollment = (await unitOfWork.EnrollmentsRepository.GetAsync(e => e.EnrollmentId == item.EnrollmentId)).FirstOrDefault();
                var course = (await unitOfWork.CoursesRepository.GetAsync(c => c.Id == enrollment.CourseId)).FirstOrDefault();
                studentGrades.Add(new StudentGrade {GradeValue=item.Value, Date = item.Date, CourseTitle=course.CourseTitle });
            }
            return studentGrades;
        }

        private async Task<IEnumerable<Grade>> GetGradesAsync(IEnumerable<Enrollment> enrollments)
        {
            var grades = new List<Grade>();
            foreach (var enrollment in enrollments)
            {
                var grade = (await unitOfWork.GradesRepository.GetAsync(grade => grade.EnrollmentId == enrollment.EnrollmentId && 
                    grade.Value != null)).FirstOrDefault();
                if (grade != null)
                {
                    grades.Add(grade);
                }
            }
            return grades;
        }

        public async Task<Grade> UpdateAsync(Grade grade)
        {
            bool gradeExists = await unitOfWork.GradesRepository.ExistsAsync(g => g.GradeId == grade.GradeId);
            if (!gradeExists)
            {
                throw new EntityNotFoundException($"The grade with the id {grade.GradeId} was not found!");
            }

            var updatedGrade = await unitOfWork.GradesRepository.UpdateAsync(grade);
            await unitOfWork.SaveChangesAsync();

            return updatedGrade;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Grade, bool>> filter)
        {
            return await unitOfWork.GradesRepository.ExistsAsync(filter);
        }
    }
}
