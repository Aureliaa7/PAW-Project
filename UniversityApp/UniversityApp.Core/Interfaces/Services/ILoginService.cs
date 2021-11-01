using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ILoginService
    {
        public Task Login(Users user, UserManager<Users> userManager);
    }
}
