using System.Threading.Tasks;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task LoginAsync(UserLogin userLogin);
        
        Task LogoutAsync();
    }
}
