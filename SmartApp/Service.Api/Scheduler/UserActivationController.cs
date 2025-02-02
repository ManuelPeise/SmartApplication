using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Settings;

namespace Service.Api.Scheduler
{
    public class UserActivationController: MaintananceControllerBase
    {
        private readonly IUserAdministrationService _userAdministrationService;
        public UserActivationController(IUserAdministrationService userAdministrationService, IOptions<AppSettingsModel> appSettings, IEmailClient emailClient): base(appSettings, emailClient)
        {
           _userAdministrationService = userAdministrationService;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.Admin, AllowMaintananceUser = true)]
        [HttpPost(Name = "ActivateUsers")]
        public async Task ActivateUsers() 
        {
            
           await _userAdministrationService.ActivateUsers(SendMail);
        }
    }
}
