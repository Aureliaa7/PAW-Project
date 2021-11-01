using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Core.DomainServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

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

        public async Task DeleteAsync(Guid id)
        {
            bool userExists = await userRepository.ExistsAsync(u => u.Id == id.ToString());
            if (!userExists)
            {
                //TODO throw an error
            }

            await userRepository.DeleteAsync(id);
            await userRepository.SaveAsync();
        }

        public async Task<IEnumerable<Users>> GetAsync(Expression<Func<Users, bool>> filter = null)
        {
            return (await userRepository.FindAsync(filter)).ToList();
        }
    }
}
