namespace Order_api.Services.Caching;
public interface ICachingServices
{
    public Task SetCacheAsync(string key, byte[] value);

}