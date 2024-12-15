using Logic.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Identity;
using Shared.Models.Response;

namespace Service.Api.Identity
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthenticationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(Name = "Authenticate")]
        public async Task<ApiResponseBase<AuthTokenResponse>> Authenticate([FromBody] AuthenticationRequest request)
        {
            return await _identityService.AuthenticateAsync(request);
        }

        [HttpGet(Name = "Logout")]
        public async Task<ApiResponseBase<LogoutResponse>> Logout([FromQuery] int userId)
        {
            return await _identityService.LogoutAsync(userId);
        }
    }
}
