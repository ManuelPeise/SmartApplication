using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Databases.Seeds
{
    public class AdminUserSeed : IEntityTypeConfiguration<UserIdentity>
    {
        public AdminUserSeed()
        {

        }

        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            var timeStamp = DateTime.Now;
         
            builder.HasData(new UserIdentity
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin.user@gmx.de",
                IsNewUserRegistration = false,
                IsActive = true,
                CredentialsId = 1,
                RoleId = 2,
                CreatedBy = "System",
                CreatedAt = timeStamp,
                UpdatedBy = "System",
                UpdatedAt = timeStamp,
            });
        }
    }
}
