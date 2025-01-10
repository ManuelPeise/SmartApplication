using Data.Shared.Logging;
using Data.Shared.Tools;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmailAddressMappingEntity>()
                 .HasOne<EmailCleanerSettingsEntity>(s => s.EmailCleanerSettings)
                 .WithMany(m => m.EmailAddressMappings)
                 .HasForeignKey(m => m.EmailCleanerSettingsId);
        }

        public DbSet<LogMessageEntity> LogMessages { get; set; }
        public DbSet<EmailAccountEntity> EmailAccounts { get; set; }
        public DbSet<EmailCleanerSettingsEntity> EmailCleanerSettings { get; set; }
        public DbSet<EmailAddressMappingEntity> EmailAddressMappings { get; set; }
    }
}
