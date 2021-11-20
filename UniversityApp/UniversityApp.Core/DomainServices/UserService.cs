using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Repositories;

namespace UniversityApp.Core.DomainServices
{
    public class UserService
    {
        protected readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        protected async Task<List<string>> SaveUserAsync(User user, string password, string role)
        {
            // Note: First set the UserName because otherwise the insert fails.
            user.UserName = user.Email;
            var result = await userManager.CreateAsync(user, password);

            var errorMessages = new List<string>();

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                result.Errors.ToList().ForEach(e => errorMessages.Add(e.Description));
            }

            return errorMessages;
        }

        protected void SetNewPropertyValues(User existingUser, User newUser)
        {
            existingUser.LastName = newUser.LastName;
            existingUser.FirstName = newUser.FirstName;
            existingUser.PhoneNumber = newUser.PhoneNumber;
            existingUser.Cnp = newUser.Cnp;
        }
    }
}
