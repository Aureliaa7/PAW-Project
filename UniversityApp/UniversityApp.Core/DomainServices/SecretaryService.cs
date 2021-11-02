using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Interfaces;

namespace UniversityApp.Core.DomainServices
{
    public class SecretaryService : ISecretaryService
    {
        private readonly IUnitOfWork unitOfWork;

        public SecretaryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddAsync(SecretaryRegistrationViewModel secretaryModel, string userId)
        {

            Secretary secretary = new Secretary
            {
                UserId = userId,
                FirstName = secretaryModel.FirstName,
                LastName = secretaryModel.LastName,
                Cnp = secretaryModel.Cnp,
                PhoneNumber = secretaryModel.PhoneNumber,
                Email = secretaryModel.Email
            };
            await unitOfWork.SecretariesRepository.CreateAsync(secretary);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByCnpAsync(string cnp)
        {
            bool secretaryExists = await unitOfWork.SecretariesRepository.ExistsAsync(s => s.Cnp == cnp);
            if (!secretaryExists)
            {
                throw new EntityNotFoundException($"The searched secretary was not found!");
            }

            var secretaryId = (await unitOfWork.SecretariesRepository
                .FindAsync(s => s.Cnp == cnp))
                .Select(x => x.SecretaryId)
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
            bool secretaryExists = await unitOfWork.SecretariesRepository.ExistsAsync(s => s.SecretaryId == secretary.SecretaryId);
            if (!secretaryExists)
            {
                throw new EntityNotFoundException($"The searched secretary was not found!");
            }

            await unitOfWork.SecretariesRepository.UpdateAsync(secretary);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
