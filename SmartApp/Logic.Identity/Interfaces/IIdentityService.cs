using Shared.Models.Identity;
using Shared.Models.Response;

namespace Logic.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<ApiResponseBase<AuthTokenResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<ApiResponseBase<LogoutResponse>> LogoutAsync(int userId);
        Task<ApiResponseBase<SuccessResponse>> RequestAccount(AccountRequest request);
        Task<ApiResponseBase<GrantAccountRequestResult>> GrantAccountRequest(int requestId);
    }
}
