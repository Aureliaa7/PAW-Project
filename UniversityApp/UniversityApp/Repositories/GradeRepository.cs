using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class GradeRepository : RepositoryBase<Grades>, IGradeRepository
    {
        public GradeRepository(UniversityAppContext context) : base(context) { }

        public IEnumerable<Grades> GetGrades(IEnumerable<Enrollments> enrollments)
        {
            var grades = new List<Grades>();
            foreach(var enrollment in enrollments) 
            {
                var grade = FindByCondition(grade => grade.EnrollmentId == enrollment.EnrollmentId).FirstOrDefault();
                grades.Add(grade);
            }
            return grades;
        }
    }
}
