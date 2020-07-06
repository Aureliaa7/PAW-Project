using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces
{
    public interface ITeacherService
    {
        ITeacherRepository TeacherRepository { get; }
        IUserRepository UserRepository { get; }
        void RegisterTeacher(TeacherRegistrationViewModel teacherModel, string userId);
    }
}
