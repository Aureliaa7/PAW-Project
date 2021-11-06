using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.DomainServices
{
    public class SecretaryService : ISecretaryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageService imageService;
        private readonly UserManager<User> userManager;

        public SecretaryService(
            IUnitOfWork unitOfWork, 
            IImageService imageService,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
            this.userManager = userManager;
        }

        public async Task AddAsync(SecretaryRegistrationViewModel secretaryModel, IFormFile image = null)
        {
            Secretary secretary = new Secretary
            {
                FirstName = secretaryModel.FirstName,
                LastName = secretaryModel.LastName,
                Cnp = secretaryModel.Cnp,
                PhoneNumber = secretaryModel.PhoneNumber,
                Email = secretaryModel.Email,
                Image = imageService.GetBytes(image),
                UserName = secretaryModel.Email
            };

            var result = await userManager.CreateAsync(secretary, secretaryModel.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(secretary, secretaryModel.Role);
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

        public async Task DeleteByCnpAsync(string cnp)
        {
            bool secretaryExists = await unitOfWork.UsersRepository.ExistsAsync(s => s.Cnp == cnp);
            if (!secretaryExists)
            {
                throw new EntityNotFoundException($"The searched secretary was not found!");
            }

            var secretaryId = (await unitOfWork.SecretariesRepository
                .FindAsync(s => s.Cnp == cnp))
                .Select(x => x.Id)
                .FirstOrDefault();
            await unitOfWork.SecretariesRepository.DeleteAsync(secretaryId);
            await unitOfWork.SaveChangesAsync();
        }

        public Task<IQueryable<Secretary>> GetAsync(Expression<System.Func<Secretary, bool>> filter = null)
        {
            return unitOfWork.SecretariesRepository.FindAsync(filter);
        }

        public async Task UpdateAsync(Secretary secretary)
        {
            bool secretaryExists = await unitOfWork.SecretariesRepository.ExistsAsync(s => s.Id == secretary.Id);
            if (!secretaryExists)
            {
                throw new EntityNotFoundException($"The searched secretary was not found!");
            }

            await unitOfWork.SecretariesRepository.UpdateAsync(secretary);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
