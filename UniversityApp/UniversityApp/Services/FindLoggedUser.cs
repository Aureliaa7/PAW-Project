using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UniversityApp.Interfaces.Services;

namespace UniversityApp.Services
{
    public class FindLoggedUser : IFindLoggedInUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public FindLoggedUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GetIdLoggedInUser() => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
