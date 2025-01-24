using Data.ContextAccessor.Interfaces;
using Data.Databases;
using Data.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Data.ContextAccessor.Repositories
{
    public class GenericSettingsRepository
    {

        private readonly DbContextRepository<GenericSettingsEntity> _genericSettingsRepository;

        public GenericSettingsRepository(ApplicationContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericSettingsRepository = new DbContextRepository<GenericSettingsEntity>(applicationDbContext, httpContextAccessor);
        }

        public async Task<T?> GetSettings<T>(IModuleSettings moduleSettings, int userId)
        {
            var settingsEntity = await _genericSettingsRepository.GetFirstOrDefault(setting =>
                setting.ModuleType == moduleSettings.ModuleType
                && setting.ModuleName == moduleSettings.ModuleName
                && setting.UserId == userId);

            if (settingsEntity == null || string.IsNullOrEmpty(settingsEntity.SettingsJson)) 
            { 
                return default(T?);
            }

            return JsonConvert.DeserializeObject<T>(settingsEntity.SettingsJson);
        }

        public async Task<bool> SaveSettings<T>(IModuleSettings moduleSettings, int userId, T settingsModel)
        {
            var settingsEntity = await _genericSettingsRepository.GetFirstOrDefault(setting =>
                setting.ModuleType == moduleSettings.ModuleType
                && setting.ModuleName == moduleSettings.ModuleName
                && setting.UserId == userId);

            if (settingsEntity == null)
            {
                return false;
            }

            settingsEntity.SettingsJson = JsonConvert.SerializeObject(settingsModel, Formatting.Indented);

            _genericSettingsRepository.Update(settingsEntity);

            await _genericSettingsRepository.SaveChangesAsync();

            return true;
        }
    }
}
