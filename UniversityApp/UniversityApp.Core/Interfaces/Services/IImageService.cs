using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IImageService
    {
        public string GetUserProfileImage(User user);
    }
}
