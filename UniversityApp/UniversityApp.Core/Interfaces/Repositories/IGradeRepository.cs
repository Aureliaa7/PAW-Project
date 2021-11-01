using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Interfaces
{
    public interface IGradeRepository : IRepositoryBase<Grades>
    {
        // get grades based on the enrollments
        Task<IEnumerable<Grades>> GetGradesAsync(IEnumerable<Enrollments> enrollments);
    }
}
