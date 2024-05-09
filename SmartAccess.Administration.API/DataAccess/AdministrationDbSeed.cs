using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using SmartAccess.Administration.API.Model;
using System.Collections.Generic;

namespace SmartAccess.Administration.API.DataAccess
{
    public static class AdministrationDbSeed
    {
        private static readonly List<User> users =
        [
            new User{ Name= "Andriy", LastName= "Svyryd", Role = "Employee"},
            new User{ Name= "Diego", LastName= "Svyryd", Role = "Director"},
            new User{ Name= "Jane", LastName= "Austen", Role = "Manager"},
        ];

        public static readonly List<Lock> locks =
        [
            new Lock() { Name = "meeting room" },
            new Lock() { Name = "main entrance" },
            new Lock() { Name = "storage room" },
        ];

        public static readonly List<PermissionGroup> PermissionGroups =
        [
            new PermissionGroup { Name = "Office Employees", Locks = [locks[0]] },
            new PermissionGroup { Name = "Office Director", Locks = [locks[1]] },
            new PermissionGroup { Name = "Office Manager", Locks = [locks[2]] }
        ];
        public static void PopulateAdministrationDb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var context = scope.ServiceProvider.GetRequiredService<AdministrationDbContext>();
                context.Database.Migrate();

                if (!context.Locks.Any())
                {
                    context.Locks.AddRange(locks);
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(users);
                }

                if (!context.PermissionGroups.Any())
                {
                    context.PermissionGroups.AddRange(PermissionGroups);
                }
                context.SaveChanges();

                if (!context.UsersPermissionGroup.Any())
                {
                    foreach (var item in users)
                    {
                        var permissionGroup = context.PermissionGroups.First(x => x.Name.Contains(item.Role));
                        var user = context.Users.FirstOrDefault(x => x.Name == item.Name);
                        user.PermissionGroups.Add(permissionGroup);
                    }
                }
                if (!context.LockPermissions.Any())
                {
                    foreach (var item in PermissionGroups)
                    {
                        var permissionGroup = context.PermissionGroups.First(x => x.Name == item.Name);
                        var lockitem = context.Locks.First(x => x.Name == item.Locks[0].Name);
                        lockitem.PermissionGroups.Add(permissionGroup);
                    }
                }

                context.SaveChanges();
            }
        }

    }
}
