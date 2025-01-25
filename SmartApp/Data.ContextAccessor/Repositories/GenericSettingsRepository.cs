using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.ModuleSettings;
using Data.Databases;
using Data.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Enums;

namespace Data.ContextAccessor.Repositories
{
    public class GenericSettingsRepository
    {

        private readonly DbContextRepository<GenericSettingsEntity> _genericSettingsRepository;

        public GenericSettingsRepository(ApplicationContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericSettingsRepository = new DbContextRepository<GenericSettingsEntity>(applicationDbContext, httpContextAccessor);
        }

        public async Task<T?> GetSettings<T>(string moduleName, ModuleTypeEnum moduleType, int userId)
        {
            var settingsEntity = await _genericSettingsRepository.GetFirstOrDefault(setting =>
                setting.ModuleType == moduleType
                && setting.ModuleName == moduleName
                && setting.UserId == userId);

            if (settingsEntity == null || string.IsNullOrEmpty(settingsEntity.SettingsJson))
            {
                return default(T?);
            }

            return JsonConvert.DeserializeObject<T>(settingsEntity.SettingsJson);
        }

        public async Task SaveSettings<T>(string moduleName, ModuleTypeEnum moduleType, int userId, T model)
        {
            var settingsEntity = await _genericSettingsRepository.GetFirstOrDefault(setting =>
                setting.ModuleType == moduleType
                && setting.UserId == userId);

            if (settingsEntity == null)
            {
                settingsEntity = new GenericSettingsEntity
                {
                    ModuleName = EmailAccountInterfaceSettings.ModuleName,
                    ModuleType = EmailAccountInterfaceSettings.ModuleType,
                    UserId = userId,
                    SettingsJson = JsonConvert.SerializeObject(model)
                };

                await _genericSettingsRepository.AddAsync(settingsEntity);
            }
            else
            {
                settingsEntity.SettingsJson = JsonConvert.SerializeObject(model);

                _genericSettingsRepository.Update(settingsEntity);
            }

            await _genericSettingsRepository.SaveChangesAsync();

            return;
        }
    }
}

