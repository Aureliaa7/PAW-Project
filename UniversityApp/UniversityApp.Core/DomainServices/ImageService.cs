﻿using System;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    public class ImageService : IImageService
    {
        public string GetUserProfileImage(User user)
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
