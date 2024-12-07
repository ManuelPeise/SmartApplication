using Data.Identity.Seeds;
using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Identity
{
    public class IdentityDbContext : DbContext
    {
       

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           builder.ApplyConfiguration(new UserCredentialsSeed());
           builder.ApplyConfiguration(new AdminUserSeed());
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserIdentity> Users { get; set; }
        public DbSet<UserCredentials> Credentials { get; set; }
        
    }
}
