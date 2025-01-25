using Logic.Interfaces.EmailAccountInterface.Models;
using Logic.Interfaces.Interfaces;
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
        public async Task<List<EmailAccountSettings>> GetEmailAccountSettings()
        {
            return await _module.GetEmailAccountSettings();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailAccountSettings")]
        public async Task<bool> UpdateEmailAccountSettings([FromBody] EmailAccountSettings settings)
        {
            return await _module.UpdateEmailAccountSettings(settings);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "TestConnection")]
        public async Task<bool> TestConnection([FromBody] EmailAccountConnectionTestRequest request)
        {
            return await _module.ExcecuteConnectionTest(request);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailMappingTable")]
        public async Task<bool> UpdateEmailMappingTable([FromQuery] string settingsGuid)
        {
            return await _module.ExecuteEmailMappingTableUpdate(settingsGuid);
        }

    }
}
