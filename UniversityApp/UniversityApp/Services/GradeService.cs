using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Services
{
    public class GradeService : IGradeService
    {
        public GradeService(UniversityAppContext context)
        {
            EnrollmentRepository = new EnrollmentRepository(context);
            GradeRepository = new GradeRepository(context);
            StudentRepository = new StudentRepository(context);
            CourseRepository = new CourseRepository(context);
        }

        public IEnrollmentRepository EnrollmentRepository { get; }
        public IGradeRepository GradeRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public ICourseRepository CourseRepository { get; }

        //get all grades of a student based on his id
        public IEnumerable<StudentGrade> GetGradesForStudent(int studentId)
        {
            //var grades = from e in EnrollmentRepository.FindAll()
            //             join g in GradeRepository.FindAll() on e.EnrollmentId equals g.EnrollmentId // this must be replaced with find by condition
            //             join s in StudentRepository.FindAll() on e.StudentId equals s.StudentId
            //             select g;
            //return grades.ToList();

            var enrollments = EnrollmentRepository.GetEnrollments(studentId);
            var dictionary = new Dictionary<Enrollments, Courses>();
            var grades = GradeRepository.GetGrades(enrollments);
            foreach (var enrollment in enrollments)
            {
                dictionary.Add(enrollment, CourseRepository.FindByCondition(c => c.CourseId == enrollment.CourseId).FirstOrDefault());
            }
            
            var studentGrades = new List<StudentGrade>();
            foreach(var item in grades)
            {
                // search the enrollment
                var enrollment = EnrollmentRepository.FindByCondition(e => e.EnrollmentId == item.EnrollmentId).FirstOrDefault();
                // search the course
                var course = CourseRepository.FindByCondition(c => c.CourseId == enrollment.CourseId).FirstOrDefault();
                // create the studentGrade object
                studentGrades.Add(new StudentGrade {GradeValue=item.Value, Date = item.Date, CourseTitle=course.CourseTitle });
            }
            return studentGrades;
        }
    }
}
