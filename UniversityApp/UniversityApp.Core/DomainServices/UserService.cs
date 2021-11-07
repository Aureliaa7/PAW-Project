using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //TODO remove this method
        public User CreateUser(List<IFormFile> images, string userName, string email, string phoneNumber)
        {
            var user = new User
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
            bool userExists = await unitOfWork.UsersRepository.ExistsAsync(u => u.Id == id);
            if (!userExists)
            {
                throw new EntityNotFoundException($"The user with the id {id} was not found!");
            }

            await unitOfWork.UsersRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAsync(Expression<Func<User, bool>> filter = null)
        {
            return (await unitOfWork.UsersRepository.GetAsync(filter)).ToList();
        }
    }
}
