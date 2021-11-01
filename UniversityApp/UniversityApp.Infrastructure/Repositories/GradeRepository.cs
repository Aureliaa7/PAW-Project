using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces;

namespace UniversityApp.Repositories
{
    public class GradeRepository : RepositoryBase<Grades>, IGradeRepository
    {
        public GradeRepository(UniversityAppContext context) : base(context) { }

        public async Task<IEnumerable<Grades>> GetGradesAsync(IEnumerable<Enrollments> enrollments)
        {
            var grades = new List<Grades>();
            foreach(var enrollment in enrollments) 
            {
                var grade = (await FindAsync(grade => grade.EnrollmentId == enrollment.EnrollmentId)).FirstOrDefault();
                grades.Add(grade);
            }
            return grades;
        }
    }
}
