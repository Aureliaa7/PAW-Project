using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Models;

namespace UniversityApp.Interfaces
{
    public interface ISecretaryRepository : IRepositoryBase<Secretaries>
    {
        void AddSecretary(Secretaries secretary);
    }
}
