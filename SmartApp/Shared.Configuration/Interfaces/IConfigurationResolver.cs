namespace Shared.Configuration.Interfaces
{
    public interface IConfigurationResolver
    {
        T? GetModel<T>(string key);
    }
}
