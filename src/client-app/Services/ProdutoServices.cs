using System.Globalization;
using System.Text;

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

        _uri = _configuration["PRODUTO:URL"];
        _apiVersion = _configuration["PRODUTO:VERSION"];
        _uri = $"{_uri}/api/{_apiVersion}/Produto";
    }

    public async Task<ProdutoModel> GetProdutoPorUuidAsync(string uuid)
    {
        string? response = await _httpClient.GetStringAsync(requestUri: $"{_uri}/{uuid}");

        using JsonDocument? jsonProduto = JsonDocument.Parse(response);

        if (jsonProduto.RootElement.ValueKind is JsonValueKind.Object)
        {
            JsonElement produto = jsonProduto.RootElement;

            return ConverterJsonParaObj(produto);
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
            ProdutoModel ObjConvertido = ConverterJsonParaObj(produto);

            // Mesmo se as outras informações existirem,
            // sem o UUID não é possivel continuar o processo
            // então não adiciona o produto
            if (!string.IsNullOrEmpty(ObjConvertido.Uuid))
                productList.Add(ObjConvertido);
        }

        return productList;
    }

    public async Task PublicaMensagemAsync(OrderModel order)
    {
        // Serializa o objeto OrderModel para JSON
        var json = JsonSerializer.Serialize(order);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync("http://localhost:5086/api/v1/Message", content);
    }

    private ProdutoModel ConverterJsonParaObj(JsonElement produto)
    {
        string precoString = produto.GetProperty("price").ToString();

        bool isParsed;
        decimal price;

        try
        {
            // Rodando no windows, é necessario usar essa linha para fazer o parse corretamente,
            // já que além de ignorar o primeiro digito se fosse 0, fazendo "0,99" ser "99",
            // iria também não retornar o simbolo ',', fazendo "5,22" ser "522",
            // mas o GetCultureInfo estava estourando excessão no linux, por isso o catch
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
                Nome = produto.GetProperty("product").ToString(),
                Preco = price
            };
        }

        return new ProdutoModel();
    }
}