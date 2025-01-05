using Logic.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Identity;

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
        public async Task<string> Authenticate([FromBody] AuthenticationRequest request)
        {
            return await _identityService.AuthenticateAsync(request);
        }

        [HttpGet(Name = "Logout")]
        public async Task<bool> Logout([FromQuery] int userId)
        {
            return await _identityService.LogoutAsync(userId);
        }
    }
}
