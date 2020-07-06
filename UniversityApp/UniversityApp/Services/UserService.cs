using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Services;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Services
{
    public class UserService : IUserService
    {
        public Users CreateUser(List<IFormFile> images, string userName, string email, string phoneNumber)
        {
            var user = new Users
            {
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber
            };
            foreach (var item in images)
            {
                if(item.Length > 0)
                {
                    using(var stream = new MemoryStream())
                    {
                        item.CopyTo(stream);
                        user.Image = stream.ToArray();
                    }
                }
            }
            return user;
        }
    }
}
