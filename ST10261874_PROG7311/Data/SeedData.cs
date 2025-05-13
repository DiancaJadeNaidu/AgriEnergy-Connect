using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using ST10261874_PROG7311.Models;

namespace ST10261874_PROG7311.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //ensure the roles exist!
            string[] roleNames = { "Farmer", "Employee" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //create default Farmer user, every farmer logs in using this login
            var farmerUser = new ApplicationUser
            {
                UserName = "farmer@gmail.com",
                Email = "farmer@gmail.com"
            };
            if (userManager.Users.All(u => u.UserName != farmerUser.UserName))
            {
                var result = await userManager.CreateAsync(farmerUser, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmerUser, "Farmer");
                }
            }

            //create default Employee user, same as above
            var employeeUser = new ApplicationUser
            {
                UserName = "employee@gmail.com",
                Email = "employee@gmail.com"
            };
            if (userManager.Users.All(u => u.UserName != employeeUser.UserName))
            {
                var result = await userManager.CreateAsync(employeeUser, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, "Employee");
                }
            }
        }
    }
}
