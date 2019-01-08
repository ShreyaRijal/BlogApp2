//Model class for comment objects.
using System.ComponentModel.DataAnnotations;

namespace BlogApp2.Models
{
    public class CommentModel
    {
        [Key]
        [Editable(false)]
        public int CommentsId { get; set; }

        [MinLength(2)]
        [MaxLength(400)]
        [Required]
        public string CommentText { get; set; }

        [Required]
        [Editable(false)]
        public string BlogCommentedOnID { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
