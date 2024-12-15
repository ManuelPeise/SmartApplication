using Data.Shared.Email;
using Data.Shared.Logging;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<EmailAccountSettingsEntity> EmailAccountSettings { get; set; }
        public DbSet<LogMessageEntity> LogMessages { get; set; }
    }
}
