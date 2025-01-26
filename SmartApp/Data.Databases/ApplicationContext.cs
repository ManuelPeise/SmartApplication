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
            modelBuilder.Entity<EmailAddressEntity>()
                .HasMany(e => e.MappingEntities)
                .WithOne(e => e.AddressEntity)
                .HasForeignKey(e => e.AddressId);

            modelBuilder.Entity<EmailSubjectEntity>()
               .HasMany(e => e.MappingEntities)
               .WithOne(e => e.SubjectEntity)
               .HasForeignKey(e => e.SubjectId);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        public DbSet<GenericSettingsEntity> GenericSettingsTable { get; set; }

        public DbSet<EmailSubjectEntity> EmailSubjectTable { get; set; }
        public DbSet<EmailAddressEntity> EmailAddressTable { get; set; }
        public DbSet<EmailMappingEntity> EmailMappingTable { get; set; }


        // TODO Check that 
        public DbSet<EmailDataEntity> EmailDataTable { get; set; }
    }
}
