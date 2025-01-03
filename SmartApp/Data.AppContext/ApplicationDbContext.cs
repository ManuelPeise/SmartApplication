using Data.Shared;
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

        
        public DbSet<LogMessageEntity> LogMessages { get; set; }
        public DbSet<EmailAccountEntity> EmailAccounts { get; set; }
    }
}
