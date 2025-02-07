﻿using Data.ContextAccessor.Repositories;
using Data.Shared.Email;
using Data.Shared.Logging;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor.Interfaces
{
    public interface IApplicationUnitOfWork : IDisposable
    {
        int CurrentUserId { get; }
        public string CurrentUserName { get; }
        DbContextRepository<LogMessageEntity> LogMessageRepository { get; }
        DbContextRepository<EmailSubjectEntity> EmailSubjectTable { get; }
        DbContextRepository<EmailAddressEntity> EmailAddressTable { get; }
        DbContextRepository<EmailTargetFolderEntity> EmailTargetFolderTable { get; }
        DbContextRepository<EmailAccountEntity> EmailAccountsTable { get; }
        DbContextRepository<EmailCleanerSettingsEntity> EmailCleanerSettingsTable { get; }
        DbContextRepository<EmailCleanupConfigurationEntity> EmailCleanupConfigurationTable { get; }

        IdentityRepository IdentityRepository { get; }
        ClaimsAccessor ClaimsAccessor { get; }
        bool IsAuthenticated { get; }
        IOptions<SecurityData> SecurityData { get; }
        Task<int> SaveApplicationContextChangesAsync();
        Task<int> SaveIdentityContextChangesAsync();
    }
}
