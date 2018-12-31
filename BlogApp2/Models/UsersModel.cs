using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class UsersModel: IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }

    }
}
