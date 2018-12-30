using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyBlog.Models
{
    public class CompanyRole : IdentityRole
    {
        public CompanyRole() : base() { }

        public CompanyRole(string roleName) : base(roleName)
        {
            base.Name = roleName;
        }

    }
}
