using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Configuration.Interfaces;

namespace Shared.Configuration
{
    public class ConfigurationResolver<T> : IConfigurationResolver
    {
        private IConfiguration _configuration;
        public ConfigurationResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T? GetModel<T>(string key)
        {
            var modelJson = _configuration.GetSection(key);

            if (modelJson == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(modelJson.Value)?? default(T);
        }
    }
}
