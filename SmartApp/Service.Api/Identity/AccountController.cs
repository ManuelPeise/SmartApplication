using Logic.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Identity;
using Shared.Models.Response;

namespace Service.Api.Identity
{
    public class AccountController: ApiControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(Name = "RequestAccount")]
        public async Task<ApiResponseBase<SuccessResponse>> Authenticate([FromBody] AccountRequest request)
        {
            return await _identityService.RequestAccount(request);
        }
    }
}
