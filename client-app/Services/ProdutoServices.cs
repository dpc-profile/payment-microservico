namespace client_app.Services;
public class ProdutoServices : IProdutoServices
{

    public Task<ProdutoModel> GetProdutoPorUuidAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProdutoModel>> GetProdutosAsync()
    {
        IEnumerable<ProdutoModel>? productList = null;

        HttpClient? httpClient = new();

        HttpResponseMessage? response = await httpClient.GetAsync(requestUri: "https://localhost:7056/api/v1/Produto");

        string? data = await response.Content.ReadAsStringAsync();

        return productList;
    }
}
