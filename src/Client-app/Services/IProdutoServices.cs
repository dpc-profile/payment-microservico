namespace client_app.Services;
public interface IProdutoServices
{
    public Task<ProdutoModel> GetProdutoPorUuidAsync(string uuid);
    public Task<IEnumerable<ProdutoModel>> GetProdutosAsync();
    public Task PublicaMensagemAsync(OrderModel order);
}
