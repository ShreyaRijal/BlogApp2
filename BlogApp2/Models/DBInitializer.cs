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


        public static void Intialize(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            CreateUsers(userManager);
            // await CreateRoles(roleManager);
        }

        private static void CreateData(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            context.Blogs.Add(new BlogModel() { BlogTitle = "Test", BlogEntry = "Entry Test" });
            context.SaveChanges();
        }

        private static void CreateUsers(UserManager<IdentityUser> userManager)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = "shreyaAdmin@email.com",
                Email = "shreyaAdmin@email.com"
            };

            userManager.CreateAsync(user, "P@ssword123!").Wait();
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

        //    private static async Task<bool> Roles(ApplicationDbContext context)
        //    {
        //        IdentityResult ir;
        //        var rs = new RoleStore<IdentityRole>(context);
        //        var rm = new RoleManager<IdentityRole>(rs);

        //        ir = await rm.CreateAsync(new IdentityRole("blogger"));

        //        var us = new UserStore<UsersModel>(context);
        //        var um = new UserManager<>(us);


        //        var user = new UsersModel()
        //        {
        //            Email = "canblog@email.com",
        //        };

        //        ir = await um.CreateAsync(user, "P@ssword123!");

        //        if (ir.Succeeded == false)
        //        {
        //            return ir.Succeeded;
        //        }

        //        ir = await um.AddToRoleAsync(user, "blogger");
        //        return ir.Succeeded;
        //    }
        //}
    }
}
