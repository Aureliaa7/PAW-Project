using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Services
{
    public class TeacherService : ITeacherService
    {
        public TeacherService(UniversityAppContext context)
        {
            TeacherRepository = new TeacherRepository(context);
            UserRepository = new UserRepository(context);
        }

        public ITeacherRepository TeacherRepository { get; }

        public IUserRepository UserRepository { get; }

        public void RegisterTeacher(TeacherRegistrationViewModel teacherModel, string userId)
        {
            Teachers teacher = new Teachers
            {
                UserId = userId,
                FirstName = teacherModel.FirstName,
                LastName = teacherModel.LastName,
                Cnp = teacherModel.Cnp,
                PhoneNumber = teacherModel.PhoneNumber,
                Email = teacherModel.Email,
                Degree = teacherModel.Degree
            };
            TeacherRepository.AddTeacher(teacher);
        }
    }
}
