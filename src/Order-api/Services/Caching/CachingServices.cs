namespace Order_api.Services.Caching;

public class CachingServices : ICachingServices
{
    private readonly IDistributedCache _cache;

    public CachingServices(IDistributedCache cache)
    {
        _cache = cache;
    }

    public Task<string> GetCacheAsync(string pedidoUuid)
    {
        throw new NotImplementedException();
        // return await _cache.GetStringAsync(pedidoUuid);
    }

    public async Task SetCacheAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value);
    }
}