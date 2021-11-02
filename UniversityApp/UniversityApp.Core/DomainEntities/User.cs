using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public partial class User : IdentityUser
    {
        public User()
        {
            Secretaries = new HashSet<Secretary>();
            Students = new HashSet<Student>();
            Teachers = new HashSet<Teacher>();
        }
        
        public byte[] Image { get; set; }

        public virtual ICollection<Secretary> Secretaries { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
