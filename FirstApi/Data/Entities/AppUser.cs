using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Phone { get; set; }
    }
}
