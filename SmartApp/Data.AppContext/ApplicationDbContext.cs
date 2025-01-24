using Data.Shared.Logging;
using Data.Shared.Tools;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }

        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        public DbSet<EmailAccountEntity> EmailAccountsTable { get; set; }
        public DbSet<EmailCleanerSettingsEntity> EmailCleanerSettingsTable { get; set; }
        public DbSet<EmailAddressMappingEntity> EmailAddressMappingTable { get; set; }
        public DbSet<EmailDataEntity> EmailDataTable { get; set; }
    }
}
