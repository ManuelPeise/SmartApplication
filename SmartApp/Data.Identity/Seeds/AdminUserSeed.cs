using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace Data.Identity.Seeds
{
    public class AdminUserSeed : IEntityTypeConfiguration<UserIdentity>
    {
        public AdminUserSeed()
        {

        }

        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            var timeStamp = DateTime.Now;
         
            var salt = Guid.NewGuid().ToString();

            builder.HasData(new UserIdentity
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin.user@gmx.de",
                IsActive = true,
                CredentialsId = 1,
                RoleId = 2,
                CreatedBy = "System",
                CreatedAt = timeStamp,
                UpdatedBy = "System",
                UpdatedAt = timeStamp,
            });
        }

        private string GetEncodedPassword(string password, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password).ToList();
            passwordBytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(passwordBytes.ToArray());
        }
    }
}
