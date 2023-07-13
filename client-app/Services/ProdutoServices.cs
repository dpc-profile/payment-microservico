namespace client_app.Services;
public class ProdutoServices : IProdutoServices
{
    private readonly HttpClient _httpClient;

    public ProdutoServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProdutoModel> GetProdutoPorUuidAsync(string uuid)
    {
        string? response = await _httpClient.GetStringAsync(requestUri: $"http://localhost:5034/api/v1/Produto/{uuid}");

        using JsonDocument? jsonProduto = JsonDocument.Parse(response);

        if (jsonProduto.RootElement.ValueKind is JsonValueKind.Object)
        {
            JsonElement produto = jsonProduto.RootElement;

            string precoString = produto.GetProperty("price").ToString();

            if (decimal.TryParse(precoString, result: out decimal price))
            {
                return new ProdutoModel
                {
                    Uuid = produto.GetProperty("uuid").ToString(),
                    Produto = produto.GetProperty("product").ToString(),
                    Preco = price
                };
            }
        }

        return new ProdutoModel();
    }

    public async Task<IEnumerable<ProdutoModel>> GetProdutosAsync()
    {
        string? response = await _httpClient.GetStringAsync(requestUri: "http://localhost:5034/api/v1/Produto");

        using JsonDocument? produtos = JsonDocument.Parse(response);

        List<ProdutoModel>? productList = new();

        foreach (JsonElement produto in produtos.RootElement.EnumerateArray())
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