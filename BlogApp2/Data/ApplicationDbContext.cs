using System;
using System.Collections.Generic;
using System.Text;
using BlogApp2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlogModel> Blogs { get; set; }

        public DbSet<CommentModel> Comments { get; set; }
    }
}
