using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public partial class Users : IdentityUser
    {
        public Users()
        {
            Secretaries = new HashSet<Secretaries>();
            Students = new HashSet<Students>();
            Teachers = new HashSet<Teachers>();
        }
        
        public byte[] Image { get; set; }

        public virtual ICollection<Secretaries> Secretaries { get; set; }
        public virtual ICollection<Students> Students { get; set; }
        public virtual ICollection<Teachers> Teachers { get; set; }
    }
}
