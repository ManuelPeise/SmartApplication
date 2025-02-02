using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api.Interfaces
{
    public class EmailClassificationController: ApiControllerBase
    {
        private readonly IEmailCleanerConfigurationModule _configurationModule;

        public EmailClassificationController(IEmailCleanerConfigurationModule configurationModule)
        {
            _configurationModule = configurationModule;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name ="GetSpamClassificationData")]
        public async Task<EmailClassificationPageModel?> GetSpamClassificationData([FromQuery] int accountId)
        {
            return await _configurationModule.LoadConfigurationData(accountId);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateSpamClassificationData")]
        public async Task<bool> UpdateSpamClassificationData([FromBody] List<EmailClassificationModel> items)
        {
            return await _configurationModule.UpdateConfigurations(items);
        }
    }
}
