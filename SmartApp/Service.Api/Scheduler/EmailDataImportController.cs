using Logic.Administration.Interfaces;
using Logic.Interfaces.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Settings;

namespace Service.Api.Scheduler
{
    public class EmailDataImportController: MaintananceControllerBase
    {
       
        private readonly IEmailCleanerImportModule _importModule;

        public EmailDataImportController(IUserAdministrationService userAdministrationService, IEmailCleanerImportModule importModule, IOptions<AppSettingsModel> appSettings, IEmailClient emailClient) : base(appSettings, emailClient)
        {
            _importModule = importModule;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.Admin, AllowMaintananceUser = true)]
        [HttpPost(Name = "ImportEmailData")]
        public async Task ImportEmailData()
        {

            await _importModule.ImportAll();
        }
    }
}
