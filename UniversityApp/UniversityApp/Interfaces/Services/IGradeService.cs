using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces
{
    public interface IGradeService
    {
        IEnrollmentRepository EnrollmentRepository { get; }
        IGradeRepository GradeRepository { get; }
        IStudentRepository StudentRepository { get; }
        ICourseRepository CourseRepository { get; }
        IEnumerable<StudentGrade> GetGradesForStudent(int studentId);
    }
}
