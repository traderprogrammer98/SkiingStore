using Microsoft.AspNetCore.Identity;
using SkiingStore.Entities;

namespace SkiingStore.Data.Initializers
{
    public static class InitializeAppDbContext
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            if (!userManager.Users.Any())
            {
                var member = new User
                {
                    UserName = "bob",
                    Email = "bob@test.com"
                };
                await userManager.CreateAsync(member, "1");
                await userManager.AddToRoleAsync(member, "Member");
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };
                await userManager.CreateAsync(admin, "1");
                await userManager.AddToRolesAsync(admin, new[] {"Admin", "Member"});
            }
        }

    }
}
