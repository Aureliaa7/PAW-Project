using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces
{
    public interface IStudentRepository : IRepositoryBase<Students>
    {
        void AddStudent(Students student);
    }
}
