using Application.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Define the default admin user
            var adminEmail = "admin@gmail.com";
            var existingUser = await userManager.FindByEmailAsync(adminEmail);

            if (existingUser == null)
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var createResult = await userManager.CreateAsync(defaultUser, "Admin123!");

                if (createResult.Succeeded)
                {
                    // Retrieve the newly created user to ensure it has an ID
                    var newUser = await userManager.FindByEmailAsync(adminEmail);

                    if (newUser != null)
                    {
                        // Ensure roles exist before assigning
                        foreach (var role in new[] { Roles.Admin })
                        {
                            var roleName = role.ToString();
                            if (!await roleManager.RoleExistsAsync(roleName))
                            {
                                await roleManager.CreateAsync(new IdentityRole(roleName));
                            }
                            await userManager.AddToRoleAsync(newUser, roleName);
                        }
                    }
                }
                else
                {
                    // Log or handle user creation failure
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new System.Exception($"Failed to create admin: {errors}");
                }
            }
        }
    }
}
