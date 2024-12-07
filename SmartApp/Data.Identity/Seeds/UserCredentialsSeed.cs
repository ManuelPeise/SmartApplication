using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace Data.Identity.Seeds
{
    public class UserCredentialsSeed : IEntityTypeConfiguration<UserCredentials>
    {
        public UserCredentialsSeed()
        {

        }

        public void Configure(EntityTypeBuilder<UserCredentials> builder)
        {
            var timeStamp = DateTime.Now;
            var salt = Guid.NewGuid().ToString();

            builder.HasData(new UserCredentials
            {
                Id = 1,
                Salt = salt,
                Password = GetEncodedPassword("SuperSecret", salt),
                ExpiresAt = timeStamp.AddMonths(3),
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
