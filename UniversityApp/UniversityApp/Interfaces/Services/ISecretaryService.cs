using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces
{
    public interface ISecretaryService
    {
        ISecretaryRepository SecretaryRepository { get; }
        IUserRepository UserRepository { get; }
        void RegisterSecretary(SecretaryRegistrationViewModel secretaryModel, string userId);
        void DeleteByCnp(string cnp);
    }
}
