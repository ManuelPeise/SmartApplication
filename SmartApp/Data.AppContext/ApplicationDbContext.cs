using Data.Shared.Logging;
using Data.Shared.Settings;
using Microsoft.EntityFrameworkCore;

namespace Data.AppContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<GenericSettingsEntity> GenericSettings { get; set; }
        public DbSet<LogMessageEntity> LogMessages { get; set; }
    }
}
