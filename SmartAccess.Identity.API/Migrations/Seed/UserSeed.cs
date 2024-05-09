using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAccess.Services.Identity.Data;
using SmartAccess.Services.Identity.Model;

namespace SmartAccess.Identity.API.Migrations.Seed
{
    public static class UserSeed
    {
        private static readonly List<string> rolesData = ["Admin", "Employee", "Manager"];
        private static readonly List<(ApplicationUser User, IdentityRole Role, string Password)> usersSeedData =
            [
                (new ApplicationUser
                {
                    UserName =  "admin@example.com",
                    Email =  "admin@example.com",
                    EmailConfirmed = true,
                    Name = "administrator",
                    LastName = "senior",
                }, new IdentityRole
                {
                    Name = "Admin"
                }, "AdminSecret@123"),
                (new ApplicationUser
                {
                    UserName =  "employee@example.com",
                    Email =  "employee@example.com",
                    EmailConfirmed = true,
                    Name = "employee",
                    LastName = "senior",
                }, new IdentityRole
                {
                    Name = "Employee"
                }, "EmployeeSecret@123"),
                (new ApplicationUser
                {
                    UserName =  "manager@example.com",
                    Email =  "manager@example.com",
                    EmailConfirmed = true,
                    Name = "manager",
                    LastName = "senior",
                }, new IdentityRole
                {
                    Name = "Manager"
                }, "ManagerSecret@123")
            ];

        public static void SeedUserStoreForDashboard(this IApplicationBuilder app)
        {

            SeedStore(app).GetAwaiter().GetResult();
        }

        private async static Task SeedStore(IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();


                foreach (var role in rolesData)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                foreach (var userSeed in usersSeedData)
                {

                    ApplicationUser defaultUser = await userManager.FindByEmailAsync(userSeed.User.Email);
                    if (defaultUser == null)
                    {
                        var identityUser = new ApplicationUser { 
                            Email = userSeed.User.Email,
                            UserName = userSeed.User.UserName, 
                            EmailConfirmed = userSeed.User.EmailConfirmed, 
                            Name = userSeed.User.Name,
                            LastName = userSeed.User.LastName,
                            Expiration = "2d" };
                        await userManager.CreateAsync(identityUser);
                        var identityResult = userManager.FindByEmailAsync(identityUser.Email).Result;
                        if (identityResult != null)
                        {
                            await userManager.AddPasswordAsync(identityResult, userSeed.Password);
                            if (!await userManager.IsInRoleAsync(identityResult, userSeed.Role.Name))
                            {
                                await userManager.AddToRoleAsync(identityResult, userSeed.Role.Name);
                            }

                        }
                    }
                }
            }
        }
    }
}

