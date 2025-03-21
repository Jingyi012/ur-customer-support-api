using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if the user already exists
            var existingUser = await userManager.FindByEmailAsync("basicuser@gmail.com");

            if (existingUser == null)
            {
                // Create the default user
                var defaultUser = new ApplicationUser
                {
                    UserName = "basicuser",
                    Email = "basicuser@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var createResult = await userManager.CreateAsync(defaultUser, "Admin123!");

                if (createResult.Succeeded)
                {
                    // Fetch the newly created user again to ensure the Id is populated
                    var newUser = await userManager.FindByEmailAsync(defaultUser.Email);

                    if (newUser != null)
                    {
                        // Ensure role exists before assigning
                        var roleExists = await roleManager.RoleExistsAsync(Roles.Basic.ToString());
                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
                        }

                        // Assign the user to the role
                        await userManager.AddToRoleAsync(newUser, Roles.Basic.ToString());
                    }
                }
                else
                {
                    // Log errors if user creation fails
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user: {errors}");
                }
            }
        }
    }
}
