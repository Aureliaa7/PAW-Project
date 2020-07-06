using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces.Services
{
    public interface IUserService
    {
        Users CreateUser(List<IFormFile> image, string userName, string email, string phoneNumber);
    }
}
