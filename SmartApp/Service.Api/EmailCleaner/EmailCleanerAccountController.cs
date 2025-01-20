using Data.Shared.Tools;
using Logic.EmailCleaner.Interfaces;
using Logic.EmailCleaner.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Settings.EmailAccountMappings;

namespace Service.Api.EmailCleaner
{

    public class EmailCleanerAccountController : ApiControllerBase
    {
        private readonly IEmailCleanerService _emailCleanerService;

        public EmailCleanerAccountController(IEmailCleanerService emailCleanerService)
        {
            _emailCleanerService = emailCleanerService;
        }


        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetEmailAccounts")]
        public async Task<List<EmailAccountModel>> GetEmailAccounts([FromQuery] bool loadMappings)
        {
            return await _emailCleanerService.GetSettings();
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpGet(Name = "GetFolderUpdate")]
        public async Task<List<FolderSettings>> GetFolderUpdate(int accountId)
        {
            return await _emailCleanerService.GetUpdatedFolderSettings(accountId);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "TestAccountConnection")]
        public async Task<bool> TestAccountConnection([FromBody] ConnectionTestModel model)
        {
            return await _emailCleanerService.TestConnection(model);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "SaveAccount")]
        public async Task<bool> SaveAccount([FromBody] EmailAccountModel model)
        {
            return await _emailCleanerService.SaveAccount(model);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateAccount")]
        public async Task<bool> UpdateAccount([FromBody] EmailAccountModel model)
        {
            return await _emailCleanerService.UpdateAccount(model);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateSettings")]
        public async Task<bool> UpdateSettings([FromBody] EmailCleanerSettings model)
        {
            return await _emailCleanerService.UpdateSettings(model);
        }

        [RoleAuthorization(RequiredRole = UserRoleEnum.User, AllowAdmin = true)]
        [HttpPost(Name = "UpdateEmailAddressMappingEntries")]
        public async Task<bool> UpdateEmailAddressMappingEntries([FromBody] List<EmailMappingModel> mappingEntries)
        {
            return await _emailCleanerService.UpdateEmailAddressMappingEntries(mappingEntries);
        }
    }
}
