using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    public class FindLoggedUser : ILoggedInUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public FindLoggedUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserId() => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
