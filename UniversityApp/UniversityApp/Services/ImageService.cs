using System;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;

namespace UniversityApp.Services
{
    public class ImageService : IImageService
    {
        public string GetUserProfileImage(Users user)
        {
            string imageDataURL = "";
            if (user.Image != null)
            {
                string imageBase64Data = Convert.ToBase64String(user.Image, 0, user.Image.Length);
                imageDataURL = string.Format(@"data:image/png;base64,{0}", imageBase64Data);
            }
            return imageDataURL;
        }
    }
}
