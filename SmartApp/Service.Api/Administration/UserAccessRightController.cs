using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration.AccessRights;

namespace Service.Api.Administration
{
    public class UserAccessRightController: ApiControllerBase
    {
        private readonly IAccessRightAdministrationService _accessRightAdministrationService;

        public UserAccessRightController(IAccessRightAdministrationService accessRightAdministrationService)
        {
            _accessRightAdministrationService = accessRightAdministrationService;
        }

        [HttpGet(Name = "LoadUserAccessRights")]
        public async Task<UserAccessRightModel> LoadUserAccessRights([FromQuery]int userId)
        {
            return await _accessRightAdministrationService.GetUserrights(userId);
        }

        [HttpGet(Name = "GetUsersWithAccessRights")]
        public async Task<List<UserAccessRightModel>> GetUsersWithAccessRights()
        {
            return await _accessRightAdministrationService.GetUsersWithAccessRights();
        }
    }
}
