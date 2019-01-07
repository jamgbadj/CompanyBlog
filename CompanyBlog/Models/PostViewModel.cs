using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyBlog.Models
{
    public class PostViewModel
    {
        [Key]
        public int PostID { get; set; }

        [Required(ErrorMessage ="Please give your post a title")]
        [MinLength(3), MaxLength(50)]
        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        [Required(ErrorMessage ="Please add a body to your post")]
        public string Body { get; set; } = "";


        public string CurrentImage { get; set; } = "";


        public IFormFile Image { get; set; } = null;

        public int ViewCount { get; set; }

    }
}
