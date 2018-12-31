using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class JustBlogs
    {
        public BlogModel Blog { get; set; }
        public string UserName { get; set; }
        public List<BlogModel> Blogs { get; set; }

        public JustBlogs(List<BlogModel> blogs)
        {
            this.Blogs = blogs;
        }
    }
}
