using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    //TODO get rid of this service or move it to presentation layer
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
