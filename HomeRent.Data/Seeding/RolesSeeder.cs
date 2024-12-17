using HomeRent.Data.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HomeRent.Data.Seeding
{
    public class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                
            await SeedRoleAsync(roleManager, "Administrator");
            await SeedRoleAsync(roleManager, "Owner");
            await SeedRoleAsync(roleManager, "Tenant");
            await SeedAdministratorAsync(userManager);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task SeedAdministratorAsync(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@gmail.com");

            if (user is null)
            {
                var adminUser = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    PhoneNumber = "0888888888"
                };

                string adminPassword = "Admin123";

                var createdAdmin = await userManager.CreateAsync(adminUser, adminPassword);
                if (createdAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }
}
