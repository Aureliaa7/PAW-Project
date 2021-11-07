using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    public class SecretaryService : ISecretaryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public SecretaryService(
            IUnitOfWork unitOfWork, 
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task AddAsync(Secretary secretary, string password)
        {
            // Note: First set the UserName because otherwise the insert fails.
            secretary.UserName = $"{secretary.FirstName}{secretary.LastName}";
            var result = await userManager.CreateAsync(secretary, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(secretary, Constants.SecretaryRole);
            }
            else
            {
                var errorMessages = new List<string>();

                foreach (var error in result.Errors)
                {
                    errorMessages.Add(error.Description);
                }

                throw new FailedUserRegistrationException(string.Join("\n", errorMessages));
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await CheckIfSecretaryExistsAsync(id);
            await unitOfWork.SecretariesRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByCnpAsync(string cnp)
        {
            bool secretaryExists = await unitOfWork.UsersRepository.ExistsAsync(s => s.Cnp == cnp);
            if (!secretaryExists)
            {
                throw new EntityNotFoundException($"The searched secretary was not found!");
            }

            var secretaryId = (await unitOfWork.SecretariesRepository
                .GetAsync(s => s.Cnp == cnp))
                .Select(x => x.Id)
                .FirstOrDefault();
            await unitOfWork.SecretariesRepository.DeleteAsync(secretaryId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Secretary>> GetAllAsync(Expression<Func<Secretary, bool>> filter = null)
        {
            return (await unitOfWork.SecretariesRepository.GetAsync(filter)).ToList();
        }

        public async Task<Secretary> GetFirstOrDefaultAsync(Expression<Func<Secretary, bool>> filter)
        {
            return (await unitOfWork.SecretariesRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task UpdateAsync(Secretary secretary)
        {
            await CheckIfSecretaryExistsAsync(secretary.Id);
            await unitOfWork.SecretariesRepository.UpdateAsync(secretary);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task CheckIfSecretaryExistsAsync(Guid id)
        {
            bool secretaryExists = await unitOfWork.UsersRepository.ExistsAsync(s => s.Id == id);
            if (!secretaryExists)
            {
                throw new EntityNotFoundException($"The searched secretary was not found!");
            }

        }
    }
}
