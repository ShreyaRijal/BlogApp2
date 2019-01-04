using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentsId { get; set; }

        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string CommentText { get; set; }

        public string BlogCommentedOnID { get; set; }

       public string UserName { get; set; }
    }
}
