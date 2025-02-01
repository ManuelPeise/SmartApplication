using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api.Interfaces
{
    public class EmailCleanerInterfaceController : ApiControllerBase
    {
        private readonly IEmailCleanerInterfaceModule _module;

        public EmailCleanerInterfaceController(IEmailCleanerInterfaceModule module)
        {
            _module = module;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetEmailCleanerSettings")]
        public async Task<List<EmailCleanerUiSettings>> GetEmailCleanerSettings()
        {
            return await _module.GetEmailCleanerSettings();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailCleanerSettings")]
        public async Task<bool> UpdateEmailCleanerSettings([FromBody] EmailCleanerUiSettings model)
        {
            return await _module.UpdateEmailCleanerSetting(model);
        }

    }
}
