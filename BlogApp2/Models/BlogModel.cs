using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class BlogModel
    {
        BloggersModel blogger = new BloggersModel();

        public string BloggersID()
        {
            return blogger.BloggersID;
        }

        [Key]
        [Required]
        public string BlogEntryID { get; set; }


        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string BlogTitle { get; set; }

        [MinLength (10)]
        [MaxLength(10000)]
        [Required]
        public string BlogEntry { get; set; }

        private string MakeBlogID(string BlogEntryID)
        {
            this.BlogEntryID = BlogEntryID;
            BlogEntryID = Guid.NewGuid().ToString();
            return BlogEntryID;
        }

    }
}
