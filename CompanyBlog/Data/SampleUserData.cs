using CompanyBlog.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyBlog.Data
{
    public class SampleUserData
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<CompanyUser> userManager,
            RoleManager<CompanyRole> roleManager)
        {
            context.Database.EnsureCreated();

            string adminID = "";

            string[] appRoles = new string[] { "Admin", "User" };

            string password = "Password123!";

            foreach (string role in appRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new CompanyRole(role));
                }
            }

            //Create admin user
            if (await userManager.FindByNameAsync("Member1") == null)
            {
                var user = new CompanyUser
                {
                    UserName = "Member1@email.com",
                    FirstName = "Admin",
                    LastName = "Company",
                    Email = "Member1@email.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, appRoles[0]);
                }

                user.Id = adminID;
            }

            //create customer 1
            if (await userManager.FindByNameAsync("Customer1") == null)
            {
                var user = new CompanyUser
                {
                    UserName = "Customer1@email.com",
                    FirstName = "Customer1",
                    LastName = "Company",
                    Email = "Customer1@email.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, appRoles[1]);
                }
            }

            //create customer 2
            if (await userManager.FindByNameAsync("Customer2") == null)
            {
                var user = new CompanyUser
                {
                    UserName = "Customer2@email.com",
                    FirstName = "Customer2",
                    LastName = "Company",
                    Email = "Customer2@email.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, appRoles[1]);
                }
            }

            //create customer 3
            if (await userManager.FindByNameAsync("Customer3") == null)
            {
                var user = new CompanyUser
                {
                    UserName = "Customer3@email.com",
                    FirstName = "Customer3",
                    LastName = "Company",
                    Email = "Customer3@email.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, appRoles[1]);
                }
            }

            //create customer 4
            if (await userManager.FindByNameAsync("Customer4") == null)
            {
                var user = new CompanyUser
                {
                    UserName = "Customer4@email.com",
                    FirstName = "Customer4",
                    LastName = "Company",
                    Email = "Customer4@email.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, appRoles[1]);
                }
            }

            //create customer 5
            if (await userManager.FindByNameAsync("Customer5") == null)
            {
                var user = new CompanyUser
                {
                    UserName = "Customer5@email.com",
                    FirstName = "Customer5",
                    LastName = "Company",
                    Email = "Customer5@email.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, appRoles[1]);
                }
            }
        }
    }
}
