//Model to access the Identity Framework users.
using Microsoft.AspNetCore.Identity;

namespace BlogApp2.Models
{
    public class UsersModel: IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }

    }
}
