using Logic.Settings.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Settings.EmailAccountSettings;

namespace Service.Api.Settings
{
    
    public class EmailAccountSettingsController : ApiControllerBase
    {
        private readonly IEmailAccountSettingsService _service;

        public EmailAccountSettingsController(IEmailAccountSettingsService service)
        {
            _service = service;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetEmailAccountSettings")]
        public async Task<List<EmailAccountSettingsModel>> GetEmailAccountSettings()
        {
            return await _service.GetSettings();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "SaveEmailAccountSettings")]
        public async Task<EmailAccountSettingsModel?> SaveEmailAccountSettings([FromBody]EmailAccountSettingsModel model)
        {
            return await _service.SaveOrUpdateConnection(model);
        }
    }
}
