using Data.Identity.Seeds;
using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Identity
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserCredentialsSeed());
            builder.ApplyConfiguration(new AdminUserSeed());

            builder.Entity<ModuleEntity>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Modules)
                .UsingEntity<UserModuleEntity>();

            builder.ApplyConfiguration(new AdminUserModulesSeed());
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserIdentity> Users { get; set; }
        public DbSet<UserCredentials> Credentials { get; set; }
        public DbSet<ModuleEntity> Modules { get; set; }
        public DbSet<UserModuleEntity> UserModules { get; set; }
        public DbSet<AccountRegistrationRequestEntity> RegistrationRequests { get; set; }

    }
}
