using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp2.Data;
using BlogApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlogApp2.Models
{
    public static class DbInitializer
    {
        static string[] the_roles = new string[] { "Blogger", "User" };


        public static async Task IntializeAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            //await CreateUsersAsync(userManager);
            // await CreateRoles(roleManager);
        }

        private static void CreateData(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            context.Blogs.Add(new BlogModel() { BlogTitle = "Test", BlogEntry = "Entry Test" });
            context.SaveChanges();
        }

        private static async Task CreateUsersAsync(UserManager<IdentityUser> um)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = "shreyaAdmin@email.com",
                Email = "shreyaAdmin@email.com"
            };

            // userManager.CreateAsync(user, "P@ssword123!").Wait();
            var claims = (await um.GetClaimsAsync(user)).Select(x=>x.Type);
            await um.AddClaimAsync(user, new System.Security.Claims.Claim("Admin", "true"));
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> rm)
        {
            foreach (var role in the_roles)
            {
                if (!await rm.RoleExistsAsync(role))
                {
                    var create_role = await rm.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
