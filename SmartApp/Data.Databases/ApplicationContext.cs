using Data.Shared.Logging;
using Data.Shared.Settings;
using Data.Shared.Tools;
using Microsoft.EntityFrameworkCore;

namespace Data.Databases
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }

        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        public DbSet<GenericSettingsEntity> GenericSettingsTable { get; set; }

        // TODO Check that
        public DbSet<EmailAccountEntity> EmailAccountsTable { get; set; }
        public DbSet<EmailCleanerSettingsEntity> EmailCleanerSettingsTable { get; set; }
        public DbSet<EmailAddressMappingEntity> EmailAddressMappingTable { get; set; }
        public DbSet<EmailDataEntity> EmailDataTable { get; set; }
    }
}
