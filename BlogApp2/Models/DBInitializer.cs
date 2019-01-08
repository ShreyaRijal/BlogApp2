//Initialise database with users.
using System.Linq;
using System.Threading.Tasks;
using BlogApp2.Data;
using Microsoft.AspNetCore.Identity;

namespace BlogApp2.Models
{
    public static class DbInitializer
    {

        public static void Intialize(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            CreateUsers(userManager);

        }

        //Make initial users.
        private static void CreateUsers(UserManager<IdentityUser> um)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = "Member1@email.com",
                Email = "Member1@email.com"
            };
            IdentityUser user2 = new IdentityUser
            {
                UserName = "Customer1@email.com",
                Email = "Customer1@email.com"
            };
            IdentityUser user3 = new IdentityUser
            {
                UserName = "Customer2@email.com",
                Email = "Customer2@email.com"
            };
            IdentityUser user4 = new IdentityUser
            {
                UserName = "Customer3@email.com",
                Email = "Customer3@email.com"
            };
            IdentityUser user5 = new IdentityUser
            {
                UserName = "Customer4@email.com",
                Email = "Customer4@email.com"
            };
            IdentityUser user6 = new IdentityUser
            {
                UserName = "Customer5@email.com",
                Email = "Customer5@email.com"
            };
        

            um.CreateAsync(user, "Password123!").Wait();
            um.CreateAsync(user2, "Password123!").Wait();
            um.CreateAsync(user3, "Password123!").Wait();
            um.CreateAsync(user4, "Password123!").Wait();
            um.CreateAsync(user5, "Password123!").Wait();
            um.CreateAsync(user6, "Password123!").Wait();

        }

    }
}
