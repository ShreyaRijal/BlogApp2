//This class allows CRUD functionality for the different tables listed below
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
        //Table of Blog objects.
        public DbSet<BlogModel> Blogs { get; set; }

        //Table of Comment objects.
        public DbSet<CommentModel> Comments { get; set; }

        //Table of which user likes or dislikes which blog.
        public DbSet<UserLikesOrDislikesBlog> LikesOrDislikesBlogs { get; set; }

        //Setting two keys for table UserLikesOrDislikesBlog.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLikesOrDislikesBlog>().HasKey(e => new { e.UserName, e.BlogID });

        }
    }
}
