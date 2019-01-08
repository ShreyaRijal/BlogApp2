//A model for a blog object.
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp2.Models
{
    public class BlogModel
    {
        [Required]
        public string UserName { get; set; }

        [Key]
        [Required]
        public string BlogEntryID { get; set; }

        [Required]
        public int NumOfLikes { get; set; }

        [Required]
        public int NumOfDislikes { get; set; }

        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string BlogTitle { get; set; }

        [MinLength (10)]
        [MaxLength(10000)]
        [Required]
        public string BlogEntry { get; set; }

        //Creating custom ID for blog.
        private string MakeBlogID(string BlogEntryID)
        {
            this.BlogEntryID = BlogEntryID;
            BlogEntryID = Guid.NewGuid().ToString();
            return BlogEntryID;
        }

    }
}
