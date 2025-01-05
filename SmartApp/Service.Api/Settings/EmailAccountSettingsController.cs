using Logic.Settings.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Response;
using Shared.Models.Settings.EmailAccountSettings;

namespace Service.Api.Settings
{
    [Authorize]
    public class EmailAccountSettingsController : ApiControllerBase
    {
        private readonly IEmailAccountSettingsService _service;

        public EmailAccountSettingsController(IEmailAccountSettingsService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetEmailAccountSettings")]
        public async Task<List<EmailAccountSettingsModel>> GetEmailAccountSettings()
        {
            return await _service.GetSettings();
        }

        [HttpPost(Name = "SaveEmailAccountSettings")]
        public async Task<EmailAccountSettingsModel?> SaveEmailAccountSettings([FromBody]EmailAccountSettingsModel model)
        {
            return await _service.SaveOrUpdateConnection(model);
        }


       

    }
}
