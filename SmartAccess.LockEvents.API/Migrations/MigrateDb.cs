using Microsoft.EntityFrameworkCore;
using SmartAccess.Services.LockEvents.API.Data;

namespace SmartAccess.LockEvents.API.Migrations
{
    public static class MigrateDb
    {

        public static void EnsureMigrated(this WebApplication app)
        {
            using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<LockEventsDbContext>().Database.Migrate();
        }
    }
}