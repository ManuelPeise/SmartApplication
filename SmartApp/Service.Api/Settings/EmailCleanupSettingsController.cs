using Logic.Settings.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Settings.EmailCleanerSettings;

namespace Service.Api.Settings
{
    public class EmailCleanupSettingsController:ApiControllerBase
    {
        private IEmailCleanerSettingsService _emailCleanerSettingsService;

        public EmailCleanupSettingsController(IEmailCleanerSettingsService emailCleanerSettingsService)
        {
            _emailCleanerSettingsService = emailCleanerSettingsService;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetEmailCleanerSettings")]
        public async Task<EmailCleanerConfiguration?> GetEmailCleanerSettings([FromQuery] bool loadMappings)
        {
            return await _emailCleanerSettingsService.GetSettings(loadMappings);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "SaveOrUpdateEmailCleanerSettings")]
        public async Task<bool?> SaveOrUpdateEmailCleanerSettings([FromBody] EmailCleanerSettings settings)
        {
            return await _emailCleanerSettingsService.SaveOrUpdateEmailCleanerSettings(settings);
        }
    }
}
