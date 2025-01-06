using Data.Identity.Seeds;
using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.Identity
{
    public class IdentityDbContext : DbContext
    {
        private readonly IOptions<SecurityData> _options;
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IOptions<SecurityData> securityOptions) : base(options)
        {
            _options = securityOptions;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccessRightSeed());
            builder.ApplyConfiguration(new UserCredentialsSeed(_options));
            builder.ApplyConfiguration(new AdminUserSeed());
            builder.ApplyConfiguration(new AdminAccessRightSeed());
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserIdentity> Users { get; set; }
        public DbSet<UserCredentials> Credentials { get; set; }
        public DbSet<AccessRightEntity> AccessRights { get; set; }
        public DbSet<UserAccessRightEntity> UserAccessRights { get; set; }

    }
}
