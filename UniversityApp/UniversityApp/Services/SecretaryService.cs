using System;
using System.Linq;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Services
{
    public class SecretaryService : ISecretaryService
    {
        public SecretaryService(UniversityAppContext context)
        {
            SecretaryRepository = new SecretaryRepository(context);
            UserRepository = new UserRepository(context);
        }

        public void RegisterSecretary(SecretaryRegistrationViewModel secretaryModel, string userId)
        {

            Secretaries secretary = new Secretaries
            {
                UserId = userId,
                FirstName = secretaryModel.FirstName,
                LastName = secretaryModel.LastName,
                Cnp = secretaryModel.Cnp,
                PhoneNumber = secretaryModel.PhoneNumber,
                Email = secretaryModel.Email
            };
            SecretaryRepository.AddSecretary(secretary);
        }

        public void DeleteByCnp(string cnp)
        {
            var secretaryToBeDeleted = SecretaryRepository.FindByCondition(s => String.Equals(s.Cnp, cnp)).FirstOrDefault();
            if(secretaryToBeDeleted != null)
            {
                SecretaryRepository.Delete(secretaryToBeDeleted);
            }
        }

        public ISecretaryRepository SecretaryRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }
    }
}
