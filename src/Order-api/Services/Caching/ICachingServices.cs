namespace Order_api.Services.Caching;
public interface ICachingServices
{
    public Task SetCacheAsync(string key, string value);

    public Task<string> GetCacheAsync(string pedidoUuid);
    
}