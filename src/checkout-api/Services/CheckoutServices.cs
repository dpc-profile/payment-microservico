namespace checkout_api.Services;

public class CheckoutServices : ICheckoutServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    private readonly string _produto_uri;
    private readonly string _produce_uri;

    public CheckoutServices(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;

        _produto_uri = _config["PRODUTO:URL"];
        _produto_uri = $"{_produto_uri}/api/{_config["PRODUTO:VERSION"]}/Produto";

        _produce_uri = $"http://localhost:{_config["PORTA"]}/api/v1/MessageProducer";
    }

    public async Task<ProdutoModel> ConsultarProdutoAsync(string uuid)
    {
        string? response = await _httpClient.GetStringAsync(requestUri: $"{_produto_uri}/{uuid}");

        using JsonDocument? jsonProduto = JsonDocument.Parse(response);

        if (jsonProduto.RootElement.ValueKind is JsonValueKind.Object)
        {
            JsonElement produto = jsonProduto.RootElement;

            return ConverterJsonParaObj(produto);
        }

        throw new NotFoundException("Produto não encontrado.");
    }

    public OrderMessageModel CriarOrderMessage(ProdutoModel dadosProduto, OrderModel dadosOrder)
    {
        return new OrderMessageModel()
        {
            ProdutoIUuid = dadosProduto.Uuid,
            ProdutoNome = dadosProduto.Nome,
            ProdutoPreco = dadosProduto.Preco,
            UsuarioNome = dadosOrder.UsuarioNome,
            UsuarioEmail = dadosOrder.UsuarioEmail,
            UsuarioTelefone = dadosOrder.UsuarioTelefone,
            CreatedAt = dadosOrder.CreatedAt
        };
    }

    public async Task PublicarMensagemAsync(OrderMessageModel mensagem)
    {
        // Serializa o objeto OrderModel para JSON
        string json = JsonSerializer.Serialize(mensagem);

        StringContent content = new(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync($"http://localhost:{_config["PORTA"]}/api/v1/MessageProducer", content);
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

        return isParsed
            ? new ProdutoModel
            {
                Uuid = produto.GetProperty("uuid").ToString(),
                Nome = produto.GetProperty("product").ToString(),
                Preco = price
            }
            : new ProdutoModel();
    }

}