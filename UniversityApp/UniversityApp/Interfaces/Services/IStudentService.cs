using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces
{
    public interface IStudentService
    {
        IStudentRepository StudentRepository { get; }
        IUserRepository UserRepository { get; }
        void RegisterStudent(StudentRegistrationViewModel studentModel, string userId);
        void DeleteSelectedStudent(DeleteStudentViewModel model);
    }
}
