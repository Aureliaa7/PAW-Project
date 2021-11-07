using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IGradeService : ICrudOperationsService<Grade>
    {
        Task<IEnumerable<StudentGrade>> GetGradesForStudentAsync(Guid studentId);
    }
}
