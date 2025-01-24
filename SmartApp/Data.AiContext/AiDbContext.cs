using Data.Shared.Ai;
using Microsoft.EntityFrameworkCore;

namespace Data.AiContext
{
    public class AiDbContext : DbContext
    {
        public AiDbContext(DbContextOptions<AiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<SpamClassificationTrainingDataEntity> SpamClassificationTrainingDataTable { get; set; }
        public DbSet<AiScore> AiScoreTable { get; set; }
    }
}
