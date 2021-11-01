using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Core.DomainServices
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly IUserRepository userRepository;

        public StudentService(IStudentRepository studentRepository, IUserRepository userRepository)
        {
            this.studentRepository = studentRepository;
            this.userRepository = userRepository;
        }

        public async Task AddAsync(StudentRegistrationViewModel studentModel, string userId)
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
            await studentRepository.CreateAsync(student);
            await studentRepository.SaveAsync();
        }

        //TODO get rid of this one
        public async Task DeleteAsync(DeleteStudentViewModel model)
        {
            var studentToBeDeleted = (await studentRepository.FindAsync(s => (s.StudyYear == model.StudyYear)
                && (String.Equals(s.Section, model.SectionName)) && (String.Equals(s.Cnp, model.Cnp)))).FirstOrDefault();
            if (studentToBeDeleted != null)
            {
                var user = (await userRepository.FindAsync(u => String.Equals(u.Id, studentToBeDeleted.UserId))).FirstOrDefault();
                if(user != null)
                {
                    studentToBeDeleted.User = user;
                    await studentRepository.DeleteAsync(studentToBeDeleted);
                    await studentRepository.SaveAsync();
                }
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            bool studentExists = await studentRepository.ExistsAsync(s => s.StudentId == id);
            if (!studentExists)
            {
                //TODO thow an exception
            }

            await studentRepository.DeleteAsync(id);
            await studentRepository.SaveAsync();
        }

        public async Task<IEnumerable<Students>> GetAsync(Expression<Func<Students, bool>> filter = null)
        {
            return (await studentRepository.FindAsync(filter)).ToList();
        }

        public async Task UpdateAsync(Students student)
        {
            bool studentExists = await studentRepository.ExistsAsync(s => s.StudentId == student.StudentId);
            if (!studentExists)
            {
                //TODO thow an exception
            }

            await studentRepository.DeleteAsync(student.StudentId);
            await studentRepository.SaveAsync();
        }
    }
}
