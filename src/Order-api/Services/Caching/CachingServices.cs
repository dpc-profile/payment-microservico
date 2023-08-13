namespace Order_api.Services.Caching;

public class CachingServices : ICachingServices
{
    private readonly IDistributedCache _cache;

    public CachingServices(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetCacheAsync(string key, byte[] value)
    {
        await _cache.SetAsync(key, value);
    }
}