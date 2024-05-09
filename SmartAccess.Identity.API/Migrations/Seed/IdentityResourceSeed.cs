using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace SmartAccess.Identity.API.Migrations.Seed
{
    public static class IdentityResourceSeed
    {
        public static void PopulateIdentityServer(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            context.Database.Migrate();

            foreach (var client in IdentityConfig.GetClients())
            {
                var item = context.Clients.Where(c => c.ClientId == client.ClientId).FirstOrDefault();

                if (item == null)
                {
                    var entity = client.ToEntity();
                    context.Clients.Add(entity);
                }
            }

            foreach (var resource in IdentityConfig.GetApiResources())
            {
                var item = context.ApiResources.SingleOrDefault(c => c.Name == resource.Name);

                if (item == null)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
            }

            foreach (var scope in IdentityConfig.GetApiScopes())
            {
                var item = context.ApiScopes.SingleOrDefault(c => c.Name == scope.Name);

                if (item == null)
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }
            }

            foreach (var scope in IdentityConfig.GetIdentityResources())
            {
                var item = context.IdentityResources.SingleOrDefault(c => c.Name == scope.Name);

                if (item == null)
                {
                    context.IdentityResources.Add(scope.ToEntity());
                }
            }
            context.SaveChanges();
        }
    }
}
