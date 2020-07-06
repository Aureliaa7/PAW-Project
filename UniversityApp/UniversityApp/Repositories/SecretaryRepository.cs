using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Models;

namespace UniversityApp.Repositories
{
    public class SecretaryRepository : RepositoryBase<Secretaries>, ISecretaryRepository
    {
        public SecretaryRepository(UniversityAppContext context) : base(context) { }

        public void AddSecretary(Secretaries secretary)
        {
            Create(secretary);
        }
    }
}
