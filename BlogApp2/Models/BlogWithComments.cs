using BlogApp2.Controllers;
using BlogApp2.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class BlogWithComments
    {
        public BlogModel Blog { get; set; }
        public CommentModel Comment { get; set; }

        public List<CommentModel> Comments { get; set; }

        public string BlogID { get; set; }

        public string CommentText { get; set; }

        public string UserName { get; set; }

        public BlogWithComments(CommentModel comment)
        {
            this.Comment = comment;
        }

        public BlogWithComments (BlogModel blog)
        {
            this.Blog = blog;
        }
        public BlogWithComments(BlogModel Blog, List<CommentModel> Comments)
        {
            this.Blog = Blog;
            this.Comments = Comments;
        }

    }
}
