using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Administration;

namespace Service.Api.Administration
{
    
    public class UserAdministrationController: ApiControllerBase
    {
        private readonly IUserAdministrationService _userAdministrationService;

        public UserAdministrationController(IUserAdministrationService userAdministrationService)
        {
            _userAdministrationService = userAdministrationService;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.Admin)]
        [HttpGet(Name = "LoadUsers")]
        public async Task<List<UserAdministrationUserModel>> LoadUsers()
        {
            return await _userAdministrationService.LoadUsers();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.Admin)]
        [HttpPost(Name = "UpdateUser")]
        public async Task<bool> UpdateUser([FromBody] UserAdministrationUserModel model)
        {
            return await _userAdministrationService.UpdateUser(model);
        }
    }
}
