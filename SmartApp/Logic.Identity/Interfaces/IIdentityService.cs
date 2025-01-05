using Shared.Models.Identity;

namespace Logic.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<string> AuthenticateAsync(AuthenticationRequest request);
        Task<bool> LogoutAsync(int userId);
        Task<bool> RequestAccount(AccountRequest request);
    }
}
