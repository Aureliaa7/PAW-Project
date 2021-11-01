using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Interfaces;

namespace UniversityApp.Core.DomainServices
{
    public class SecretaryService : ISecretaryService
    {
        private readonly ISecretaryRepository secretaryRepository;

        public SecretaryService(ISecretaryRepository secretaryRepository)
        {
            this.secretaryRepository = secretaryRepository;
        }

        public async Task AddAsync(SecretaryRegistrationViewModel secretaryModel, string userId)
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
            await secretaryRepository.CreateAsync(secretary);
            await secretaryRepository.SaveAsync();
        }

        public async Task DeleteByCnpAsync(string cnp)
        {
            bool secretaryExists = await secretaryRepository.ExistsAsync(s => s.Cnp == cnp);
            if (!secretaryExists)
            {
                //TODO throw a custom exception
            }

            var secretaryId = (await secretaryRepository
                .FindAsync(s => s.Cnp == cnp))
                .Select(x => x.SecretaryId)
                .FirstOrDefault();
            await secretaryRepository.DeleteAsync(secretaryId);
            await secretaryRepository.SaveAsync();
        }

        public Task<IQueryable<Secretaries>> GetAsync(Expression<System.Func<Secretaries, bool>> filter = null)
        {
            return secretaryRepository.FindAsync(filter);
        }

        public async Task UpdateAsync(Secretaries secretary)
        {
            bool secretaryExists = await secretaryRepository.ExistsAsync(s => s.SecretaryId == secretary.SecretaryId);
            if (!secretaryExists)
            {
                //TODO throw a custom exception
            }

            await secretaryRepository.UpdateAsync(secretary);
            await secretaryRepository.SaveAsync();
        }
    }
}
