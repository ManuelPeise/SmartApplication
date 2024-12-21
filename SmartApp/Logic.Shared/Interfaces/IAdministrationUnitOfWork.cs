﻿using Data.ContextAccessor.Interfaces;
using Data.Shared.Logging;
using Data.Shared.Settings;

namespace Logic.Shared.Interfaces
{
    public interface IAdministrationUnitOfWork: IDisposable
    {
        public IRepositoryBase<GenericSettingsEntity> SettingsRepository { get; }
        T? GetValueFromClaims<T>(string key);
        Task AddLogMessage(LogMessageEntity logMessageEntity);
        Task SaveChangesAsync();
    }
}
