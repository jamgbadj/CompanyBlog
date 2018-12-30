using System;
using System.Collections.Generic;
using System.Text;
using CompanyBlog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<CompanyUser, CompanyRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CompanyBlog.Models.Post> Post { get; set; }
    }
}
