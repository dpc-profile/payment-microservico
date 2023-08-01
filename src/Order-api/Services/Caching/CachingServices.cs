namespace Order_api.Services.Caching;

public class CachingServices : ICachingServices
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;

    public CachingServices(IDistributedCache cache)
    {
        _cache = cache;
        _options = new(){
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(20)
        };
    }

    public async Task<string> GetCacheAsync(string produtoUuid)
    {
        return await _cache.GetStringAsync(produtoUuid);
    }

    public async Task SetCacheAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _options);
    }
}