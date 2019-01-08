//Model to represent if a user has liked or disliked a blog.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp2.Models
{
    public class UserLikesOrDislikesBlog
    {

        [Key, Column(Order = 0)]
        [Required]
        public string UserName { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [Editable(false)]
        public string BlogID { get; set; }

        [Required]
        public bool HasLiked { get; set; }

        [Required]
        public bool HasDisliked { get; set; }

    }
}
