using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api.Interfaces
{
    public class EmailCleanerInterfaceController : ApiControllerBase
    {
        private readonly IEmailCleanerInterfaceModule _module;
        private readonly IEmailCleanerImportModule _importModule;
        public EmailCleanerInterfaceController(IEmailCleanerInterfaceModule module, IEmailCleanerImportModule importModule)
        {
            _module = module;
            _importModule = importModule;
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetEmailCleanerSettings")]
        public async Task<List<EmailCleanerUiSettings>> GetEmailCleanerSettings()
        {
            return await _module.GetEmailCleanerSettings();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailCleanerSettings")]
        public async Task<bool> UpdateEmailCleanerSettings([FromBody] EmailCleanerUiSettings model)
        {
            return await _module.UpdateEmailCleanerSetting(model);
        }

        // manual email data import
        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true, AllowMaintananceUser = false)]
        [HttpPost(Name = "ExecuteEmailDataImport")]
        public async Task ExecuteEmailDataImport([FromQuery] int accountId)
        {
            await _importModule.Import(null, accountId);
        }

    }
}
