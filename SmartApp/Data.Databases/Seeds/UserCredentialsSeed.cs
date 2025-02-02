using Data.Shared;
using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;
using System.Text;

namespace Data.Databases.Seeds
{
    public class UserCredentialsSeed : IEntityTypeConfiguration<UserCredentials>
    {
        private readonly IOptions<SecurityData> _options;

        public UserCredentialsSeed(IOptions<SecurityData> options)
        {
            _options = options;
        }

        public void Configure(EntityTypeBuilder<UserCredentials> builder)
        {
            var timeStamp = DateTime.Now;
            var passwordHandler = new PasswordHandler(_options);

            builder.HasData(new UserCredentials
            {
                Id = 1,
                Password = passwordHandler.Encrypt("SuperSecret"),
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
