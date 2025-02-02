using Data.Databases.Seeds;
using Data.Shared.Email;
using Data.Shared.Logging;
using Data.Shared.Settings;
using Data.Shared.Tools;
using Microsoft.EntityFrameworkCore;

namespace Data.Databases
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<EmailAddressEntity>()
            //    .HasMany(e => e.MappingEntities)
            //    .WithOne(e => e.AddressEntity)
            //    .HasForeignKey(e => e.AddressId);

            //modelBuilder.Entity<EmailSubjectEntity>()
            //   .HasMany(e => e.MappingEntities)
            //   .WithOne(e => e.SubjectEntity)
            //   .HasForeignKey(e => e.SubjectId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmailTargetFolderSeed());
        }


        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
     
        // email cleaner tables
        public DbSet<EmailAccountEntity> EmailAccountTable { get; set; }
        public DbSet<EmailSubjectEntity> EmailSubjectTable { get; set; }
        public DbSet<EmailAddressEntity> EmailAddressTable { get; set; }
        public DbSet<EmailCleanerSettingsEntity> EmailCleanerSettingsTable { get; set; }
        public DbSet<EmailTargetFolderEntity> EmailTargetFolderTable { get; set; }
        public DbSet<EmailCleanupConfigurationEntity> EmailCleanupTable { get; set; }



        // TODO Check that 
        public DbSet<EmailDataEntity> EmailDataTable { get; set; }
    }
}
