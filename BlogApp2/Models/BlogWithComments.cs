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

        public BlogWithComments (BlogModel Blog)
        {
            this.Blog = Blog;
        }
        public BlogWithComments(BlogModel Blog, List<CommentModel> Comments)
        {
            this.Blog = Blog;
            this.Comments = Comments;
        }

    }
}
