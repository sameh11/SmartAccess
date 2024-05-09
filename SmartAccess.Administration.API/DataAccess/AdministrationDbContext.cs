using Microsoft.EntityFrameworkCore;
using SmartAccess.Administration.API.Model;

namespace SmartAccess.Administration.API.DataAccess
{
    public class AdministrationDbContext: DbContext
    {
        public AdministrationDbContext(DbContextOptions options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.PermissionGroups)
                .WithMany(b => b.Users)
                .UsingEntity<UserPermissionGroup>();

            modelBuilder.Entity<Lock>()
                .HasMany(p => p.PermissionGroups)
                .WithMany(b => b.Locks)
                .UsingEntity<LockPermission>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<UserPermissionGroup> UsersPermissionGroup { get; set; }
        public DbSet<Lock> Locks { get; set; }
        public DbSet<LockPermission> LockPermissions { get; set; }
    }
}
