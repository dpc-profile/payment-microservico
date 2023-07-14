using System.Globalization;

namespace client_app.Services;
public class ProdutoServices : IProdutoServices
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProdutoServices> _logger;
    private readonly IConfiguration _configuration;
    
    private readonly string _uri;
    private readonly string _apiVersion;

    public ProdutoServices(ILogger<ProdutoServices> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClient;
        _uri = _configuration["PRODUTO_API"];
        _apiVersion = _configuration["API_VERSION"];
        _uri = $"{_uri}/api/{_apiVersion}/Produto";
    }

    public async Task<ProdutoModel> GetProdutoPorUuidAsync(string uuid)
    {
        string? response = await _httpClient.GetStringAsync(requestUri: $"{_uri}/{uuid}");

        using JsonDocument? jsonProduto = JsonDocument.Parse(response);

        if (jsonProduto.RootElement.ValueKind is JsonValueKind.Object)
        {
            JsonElement produto = jsonProduto.RootElement;

            string precoString = produto.GetProperty("price").ToString();

            bool isParsed;
            decimal price;

            try
            {
                // Rodando no windows, é necessario usar essa linha, para fazer o parse corretamente mas também estoura excessão no linux
                isParsed = decimal.TryParse(precoString, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out price);
            }
            catch (CultureNotFoundException)
            {
                isParsed = decimal.TryParse(precoString, out price);
            }

            if (isParsed)
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
        string? response = await _httpClient.GetStringAsync(requestUri: _uri);

        using JsonDocument? produtos = JsonDocument.Parse(response);

        List<ProdutoModel>? productList = new();

        foreach (JsonElement produto in produtos.RootElement.EnumerateArray())
        {
            string precoString = produto.GetProperty("price").ToString();
        
            bool isParsed;
            decimal price;

            try
            {
                // Rodando no windows, é necessario usar essa linha, para fazer o parse corretamente mas também estoura excessão no linux
                isParsed = decimal.TryParse(precoString, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out price);
            }
            catch (CultureNotFoundException)
            {
                isParsed = decimal.TryParse(precoString, out price);
            }

            if (isParsed)
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