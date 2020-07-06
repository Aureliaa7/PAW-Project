using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Repositories
{
    public class StudentRepository : RepositoryBase<Students>, IStudentRepository
    {
        public StudentRepository(UniversityAppContext context) : base(context) { }

        public void AddStudent(Students student)
        {
            Create(student);
        }
    }
}
