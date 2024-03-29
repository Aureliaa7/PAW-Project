﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.DomainEntities
{
    public class User : IdentityUser<Guid>
    {
        public byte[] Image { get; set; }

        public string Cnp { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get; set; }
    }
}
