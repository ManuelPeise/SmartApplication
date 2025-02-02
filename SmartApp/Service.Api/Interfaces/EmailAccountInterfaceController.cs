using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api.Interfaces
{
    public class EmailAccountInterfaceController:ApiControllerBase
    {
        private readonly IEmailAccountInterfaceModule _module;

        public EmailAccountInterfaceController(IEmailAccountInterfaceModule module)
        {
            _module = module;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetEmailAccountSettings")]
        public async Task<List<EmailAccount>> GetEmailAccountSettings()
        {
            return await _module.GetEmailAccounts();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailAccountSettings")]
        public async Task<bool> UpdateEmailAccountSettings([FromBody] EmailAccount settings)
        {
            return await _module.UpdateEmailAccount(settings);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "TestConnection")]
        public async Task<bool> TestConnection([FromBody] EmailAccountConnectionData request)
        {
            return await _module.ExcecuteConnectionTest(request);
        }
    }
}
