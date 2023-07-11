namespace client_app.Services;
public interface IProdutoServices
{
    public Task<ProdutoModel> GetProdutoPorUuidAsync() ;
    public Task<IEnumerable<ProdutoModel>> GetProdutosAsync();
}
