using Data.AppContext.Configurations;
using Data.Shared;
using Data.Shared.AccessRights;
using Data.Shared.Logging;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccessRightConfiguration());
            builder.ApplyConfiguration(new AdminAccessRightConfiguration());
        }

        // administration 
        public DbSet<LogMessageEntity> LogMessages { get; set; }
        public DbSet<AccessRightEntity> AccessRights { get; set; }
        public DbSet<UserAccessRightEntity> UserAccessRights { get; set; }
        public DbSet<EmailAccountEntity> EmailAccounts { get; set; }
    }
}
