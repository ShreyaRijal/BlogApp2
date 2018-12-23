using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class BlogList
    {
        public int NumberOfBlogs { get; set; }

        public List<BlogModel> Blogs { get; set; }

        public static implicit operator BlogList(BlogModel v)
        {
            throw new NotImplementedException();
        }
    }
}
