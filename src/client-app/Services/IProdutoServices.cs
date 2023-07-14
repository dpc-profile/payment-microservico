namespace client_app.Services;
public interface IProdutoServices
{
    public Task<ProdutoModel> GetProdutoPorUuidAsync(string uuid);
    public Task<IEnumerable<ProdutoModel>> GetProdutosAsync();

    public void PostOrder(byte[] json);
    
}
