using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyBlog.Models
{
    public class CompanyUser : IdentityUser
    {
        public CompanyUser() : base() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
