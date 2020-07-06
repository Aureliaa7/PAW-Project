using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;

namespace UniversityApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task Login(Users user, UserManager<Users> userManager)
        {
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await userManager.GetRolesAsync(user);
            identity.AddClaim(new Claim(ClaimTypes.Role, roles.ElementAt(0)));
            await httpContextAccessor.HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
        }
    }
}
