using Logic.Interfaces.EmailCleanerInterface.Models;
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
        [HttpGet(Name = "GetEmailCleanerConfigurations")]
        public async Task<List<EmailCleanerInterfaceConfigurationUiModel>> GetEmailCleanerConfigurations([FromQuery] string? loadFolderMappings)
        {

            return await _module.GetEmailCleanerConfigurations(string.IsNullOrEmpty(loadFolderMappings) ? false : bool.Parse(loadFolderMappings));
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetFolderMappingData")]
        public async Task<EmailCleanerMappingData<EmailFolderMappingData>?> GetFolderMappingData([FromQuery] string settingsGuid)
        {
            return await _module.GetFolderMappingData(settingsGuid);
        }


        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateFolderMappingData")]
        public async Task<bool> UpdateFolderMappingData([FromBody] EmailFolderMappingUpdate update)
        {
            return await _module.UpdateFolderMappings(update);
        }
      
        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailCleanerConfigurations")]
        public async Task<bool> UpdateEmailCleanerConfigurations([FromBody] EmailCleanerUpdateModel model)
        {
            return await _module.UpdateEmailCleanerConfiguration(model);
        }

    }
}
