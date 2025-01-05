namespace Data.ContextAccessor.Interfaces
{
    public interface IClaimsAccessor
    {
        T? GetClaimsValue<T>(string key);
    }
}
