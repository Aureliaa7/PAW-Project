using Microsoft.AspNetCore.Http;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IImageService
    {
        public string GetUserProfileImage(User user);

        public byte[] GetBytes(IFormFile image);
    }
}
