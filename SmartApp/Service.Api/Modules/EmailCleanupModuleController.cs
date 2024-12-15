using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration.Email;

namespace Service.Api.Modules
{
    [Authorize]
    public class EmailCleanupModuleController:ApiControllerBase
    {
        private readonly IEmailAccountCleanupModule _emailAccountCleanupModule;
        

        public EmailCleanupModuleController(IEmailAccountCleanupModule emailAccountCleanupModule)
        {
            _emailAccountCleanupModule = emailAccountCleanupModule;
        }

        [HttpGet(Name = "GetEmailCleanupSettings")]
        public async Task<EmailAccountSettings?> GetEmailCleanupSettings()
        {
            return await _emailAccountCleanupModule.GetSettings();
        }
    }
}
