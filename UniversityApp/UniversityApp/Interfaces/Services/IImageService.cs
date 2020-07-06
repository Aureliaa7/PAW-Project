using UniversityApp.Models;

namespace UniversityApp.Interfaces.Services
{
    public interface IImageService
    {
        public string GetUserProfileImage(Users user);
    }
}
