using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreTodo.Models;


namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            await EnsureTestAdminAsync(userManager);
        }

         private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);
            if(alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users.Where(
                x => x.UserName == "admin@todo.local"
            ).SingleOrDefaultAsync();

            if(testAdmin != null)return;

            testAdmin = new IdentityUser
            {
                UserName = "admin@todoApp.local",
                Email = "admin@todoApp.local"
            };
            await userManager.CreateAsync(
                testAdmin, "NotSecure1234!!"
            );
            await userManager.AddToRoleAsync(
                testAdmin,Constants.AdministratorRole
            );
        }

    }

   
}