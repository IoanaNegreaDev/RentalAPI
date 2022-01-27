using Microsoft.AspNetCore.Identity;
using RentalAPI.Models;

namespace RentalAPI.Persistance.DbSeed
{
    public class IdentitySeeder
    {
        UserManager<RentalUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        public IdentitySeeder(UserManager<RentalUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void SeedData()
        {
            SeedRoles(_roleManager);
            SeedUsers(_userManager);
        }

        private void SeedUsers(UserManager<RentalUser> userManager)
        {
            if (userManager.FindByNameAsync("testAdmin").Result == null)
            {
                var user = new RentalUser();
                user.UserName = "Admin";
                user.PhoneNumber = "0727013462";

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                var role = new IdentityRole();
                role.Name = "NormalUser";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
