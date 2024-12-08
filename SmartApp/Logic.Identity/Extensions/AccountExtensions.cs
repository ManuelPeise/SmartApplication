using Data.Shared.Identity.Entities;
using Shared.Enums;
using Shared.Models.Identity;

namespace Logic.Identity.Extensions
{
    internal static class AccountExtensions
    {
        internal static UserIdentity ToIdentityBase(this AccountRegistrationRequestEntity request, bool isAdmin = false)
        {
            return new UserIdentity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                RoleId = isAdmin ? (int)UserRoleEnum.Admin + 1 : (int)UserRoleEnum.User + 1,
                IsActive = true, 
            };
        }
    }
}
