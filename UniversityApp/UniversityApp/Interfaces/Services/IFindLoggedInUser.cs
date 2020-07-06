using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UniversityApp.Models;

namespace UniversityApp.Interfaces.Services
{
    public interface IFindLoggedInUser
    {
        string GetIdLoggedInUser();
    }
}
