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

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Title { get; set; } = "";

        [MinLength(3), MaxLength(50)]
        public string Author { get; set; } = "";

        [Required]
        public string Body { get; set; } = "";

        public IFormFile Image { get; set; } = null;
    }
}
