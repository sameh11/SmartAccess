using Microsoft.EntityFrameworkCore;
using SmartAccess.Services.LockEvents.API.Model;

namespace SmartAccess.Services.LockEvents.API.Data
{
    public class LockEventsDbContext : DbContext
    {
        public LockEventsDbContext(DbContextOptions<LockEventsDbContext> options)
            : base(options)
        {
        }

        public DbSet<LockEvent> LockEvents { get; set; }
    }
}
