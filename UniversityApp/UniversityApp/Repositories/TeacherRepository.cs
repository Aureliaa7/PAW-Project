using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class TeacherRepository : RepositoryBase<Teachers>, ITeacherRepository
    {
        public TeacherRepository(UniversityAppContext context) : base(context) { }

        public void AddTeacher(Teachers teacher)
        {
            Create(teacher);
        }
    }
}
