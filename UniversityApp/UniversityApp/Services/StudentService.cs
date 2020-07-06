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
    public class StudentService : IStudentService
    {

        public StudentService(UniversityAppContext context)
        {
            StudentRepository = new StudentRepository(context);
            UserRepository = new UserRepository(context);
        }

        public void RegisterStudent(StudentRegistrationViewModel studentModel, string userId)
        {
            Students student = new Students
            {
                UserId = userId,
                FirstName = studentModel.FirstName,
                LastName = studentModel.LastName,
                Cnp = studentModel.Cnp,
                PhoneNumber = studentModel.PhoneNumber,
                Email = studentModel.Email,
                StudyYear = studentModel.StudyYear,
                Section = studentModel.Section,
                GroupName = studentModel.GroupName
            };
            StudentRepository.AddStudent(student);
        }

        public void DeleteSelectedStudent(DeleteStudentViewModel model)
        {
            var studentToBeDeleted = StudentRepository.FindByCondition(s => (s.StudyYear == model.StudyYear)
                && (String.Equals(s.Section, model.SectionName)) && (String.Equals(s.Cnp, model.Cnp))).FirstOrDefault();
            if (studentToBeDeleted != null)
            {
                var user = UserRepository.FindByCondition(u => String.Equals(u.Id, studentToBeDeleted.UserId)).FirstOrDefault();
                if(user != null)
                {
                    studentToBeDeleted.User = user;
                    StudentRepository.Delete(studentToBeDeleted);
                }
            }
        }

        public IStudentRepository StudentRepository { get; }
        public IUserRepository UserRepository { get; }
    }
}
