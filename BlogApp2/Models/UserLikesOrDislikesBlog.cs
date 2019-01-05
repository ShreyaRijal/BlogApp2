using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class UserLikesOrDislikesBlog
    {

        [Key, Column(Order = 0)]
        [Required]
        public string UserName { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public string BlogID { get; set; }

        [Required]
        public bool HasLiked { get; set; }

        [Required]
        public bool HasDisliked { get; set; }

    }
}
