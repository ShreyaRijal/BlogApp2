using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp2.Models
{
    public class BloggersModel: UsersModel
    {
        [Required]
        [Key]
        public string BloggersID { get; set; }

        public string MakeBloggersID(string BloggersID)
        {
            this.BloggersID = BloggersID;
            BloggersID = Guid.NewGuid().ToString();
            return BloggersID;
        }
    }
}
