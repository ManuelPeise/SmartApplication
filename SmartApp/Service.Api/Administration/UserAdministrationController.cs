using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet(Name = "LoadUsers")]
        public async Task<List<UserAdministrationUserModel>> LoadUsers()
        {
            return await _userAdministrationService.LoadUsers();
        }
    }
}
