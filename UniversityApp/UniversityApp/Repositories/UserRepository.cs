using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public UserRepository(UniversityAppContext context) : base(context) { }
    }
}
