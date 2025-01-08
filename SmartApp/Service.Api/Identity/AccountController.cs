using Logic.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Identity;

namespace Service.Api.Identity
{
    public class AccountController: ApiControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost(Name = "RequestAccount")]
        public async Task<bool> RequestAccount([FromBody] AccountRequest request)
        {
            return await _identityService.RequestAccount(request);
        }
    }
}
