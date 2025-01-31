using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api.Interfaces
{
    public class FolderMappingController : ApiControllerBase
    {
        private readonly IEmailCleanerInterfaceModule _module;

        public FolderMappingController(IEmailCleanerInterfaceModule module)
        {
            _module = module;
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
        [HttpPost(Name = "ExecuteFolderMapping")]
        public async Task<bool> ExecuteFolderMapping([FromQuery] string settingsGuid)
        {
            return await _module.ExecuteFolderMapping(settingsGuid);
        }
    }
}
