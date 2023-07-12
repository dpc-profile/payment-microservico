namespace client_app.Services;
public class ProdutoServices : IProdutoServices
{
    private readonly HttpClient _httpClient;

    public ProdutoServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ProdutoModel> GetProdutoPorUuidAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProdutoModel>> GetProdutosAsync()
    {
        string? response = await _httpClient.GetStringAsync(requestUri: "http://localhost:5034/api/v1/Produto");

        using JsonDocument? produtos = JsonDocument.Parse(response);

        List<ProdutoModel>? productList = new();

        foreach (var produto in produtos.RootElement.EnumerateArray())
        {
            string precoString = produto.GetProperty("price").ToString();

            if (decimal.TryParse(precoString, out decimal price))
            {
                productList.Add(new ProdutoModel()
                {
                    Uuid = produto.GetProperty("uuid").ToString(),
                    Produto = produto.GetProperty("product").ToString(),
                    Preco = price

                });
            }
        }
        return productList;
    }
}
