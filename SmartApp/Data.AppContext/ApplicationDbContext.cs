using Data.Shared.Logging;
using Data.Shared.Tools;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<EmailCleanerMappingDataEntity>()
            //    .HasKey(e => new { e.MappingId, e.DataId });

            //builder.Entity<EmailCleanerMappingDataEntity>()
            // .HasOne(e => e.Mapping)
            // .WithMany(s => s.EmailCleanerMappingData)
            // .HasForeignKey(e => e.MappingId);

            //builder.Entity<EmailCleanerMappingDataEntity>()
            //    .HasOne(e => e.Data)
            //    .WithMany(c => c.EmailCleanerMappingData)
            //    .HasForeignKey(e => e.DataId);
        }

        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        public DbSet<EmailAccountEntity> EmailAccountsTable { get; set; }
        public DbSet<EmailCleanerSettingsEntity> EmailCleanerSettingsTable { get; set; }
        public DbSet<EmailAddressMappingEntity> EmailAddressMappingTable { get; set; }
        public DbSet<EmailDataEntity> EmailDataTable { get; set; }
    }
}
