using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.DomainServices
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signManager;

        public AccountService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            SignInManager<User> signManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.signManager = signManager;
        }
        public async Task LoginAsync(UserLogin userLogin)
        {
            var user = await userManager.FindByEmailAsync(userLogin.Email);
            if (user == null)
            {
                throw new EntityNotFoundException($"The user with the email {userLogin.Email} was not found!");
            }

            if (!await userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                throw new IncorrectCredentialsException("Wrong credentials!");
            }

            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await userManager.GetRolesAsync(user);
            identity.AddClaim(new Claim(ClaimTypes.Role, roles.FirstOrDefault()));
            await httpContextAccessor.HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
        }

        public async Task LogoutAsync()
        {
            httpContextAccessor.HttpContext.Session.Clear();   // what's stored in session?
            await signManager.SignOutAsync();
        }
    }
}
