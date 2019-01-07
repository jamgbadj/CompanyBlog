using CompanyBlog.Models.Comments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyBlog.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Title { get; set; } = "";

        [MinLength(3), MaxLength(50)]
        public string Author { get; set; } = "";

        [Required]
        public string Body { get; set; } = "";

        public string Image { get; set; } = "";

        [Display(Name = "Posted On")]
        public DateTime PostedOn { get; set; } = DateTime.Now;

        public List<MainComment> MainComments { get; set; }

        public int ViewCount { get; set; }
    }
}
