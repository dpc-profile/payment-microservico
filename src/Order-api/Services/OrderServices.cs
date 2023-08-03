namespace Order_api.Services;

public class OrderServices : IOrderServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ICachingServices _caching;

    private readonly string _produto_uri;

    public OrderServices(HttpClient httpClient, IConfiguration config, ICachingServices caching)
    {
        _httpClient = httpClient;
        _config = config;
        _caching = caching;

        _produto_uri = _config["PRODUTO:URL"];
        _produto_uri = $"{_produto_uri}/api/{_config["PRODUTO:VERSION"]}/Produto";

    }

    public async Task ValidarEAtualizarPedidoAsync(OrderModel order)
    {
        try
        {
            if (string.IsNullOrEmpty(order.ProdutoUuid))
                throw new ArgumentNullException("O UUID do produto é nulo ou vazio.");

            JsonElement produto = await ConsultarProduto(order.ProdutoUuid);

            ValidarJsonProduto(produto);

            order.ProdutoPreco = PrecoParaDecimal(produto);

        }
        catch (JsonException error)
        {
            throw new ProdutoNaoValidoException(error.Message);
        }
    }

    public async Task AprovarPedidoAsync(OrderModel order)
    {
        await SalvarPedidoAsync(order);
    }

    public async Task ProcessarPagamentoAsync(OrderModel order)
    {
        string uuid = Guid.NewGuid().ToString();
        order.PedidoUuid = uuid;

        await SalvarPedidoAsync(order);

        await PostAsync(mensagem: order, uri: "MessageProducer");
    }

    public async Task ReprocessarPagamentoAsync(OrderModel order)
    {
        // Verifica se o updateAt tem mais de 1h a partir de agora
        DateTime umaHoraDepois = order.UpdatedAt.AddHours(1);

        if (DateTime.Now > umaHoraDepois)
        {
            order.PedidoStatus = "Cancelado";

            // Postar novamente na exchange do 'Order'
            await PostAsync(mensagem: order, uri: "Order");
        }
        else 
        {
            // Se não, postar novamente na exchange 'process-card'
            await PostAsync(mensagem: order, uri: "MessageProducer");
        }        
    }

    public async Task CancelarPedidoAsync(OrderModel order)
    {
        await SalvarPedidoAsync(order);
    }

    private void ValidarJsonProduto(JsonElement produto)
    {
        if (string.IsNullOrEmpty(produto.GetProperty("product").ToString()))
            throw new ProdutoNaoValidoException("O nome do produto é nulo ou vazio");

        if (string.IsNullOrEmpty(produto.GetProperty("price").ToString()))
            throw new ProdutoNaoValidoException("O preco do produto é nulo ou vazio");
    }

    private async Task<JsonElement> ConsultarProduto(string produtoUuid)
    {
        string? response = await _httpClient.GetStringAsync(requestUri: $"{_produto_uri}/{produtoUuid}");

        using JsonDocument? jsonProduto = JsonDocument.Parse(response);

        return jsonProduto.RootElement.ValueKind is not JsonValueKind.Object
            ? throw new JsonException("Algo foi retornado da API que não é um json")
            : jsonProduto.RootElement.Clone();
    }

    private decimal PrecoParaDecimal(JsonElement produto)
    {
        string precoString = produto.GetProperty("price").ToString();

        bool Parsed;
        decimal price;

        try
        {
            // Rodando no windows, é necessario usar essa linha para fazer o parse corretamente,
            // já que além de ignorar o primeiro digito se fosse 0, fazendo "0,99" ser "99",
            // iria também não retornar o simbolo ',', fazendo "5,22" ser "522",
            // mas o GetCultureInfo estava estourando excessão no linux, por isso o catch
            Parsed = decimal.TryParse(precoString, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out price);
        }
        catch (CultureNotFoundException)
        {
            Parsed = decimal.TryParse(precoString, out price);
        }

        if (!Parsed) throw new FormatException("Não foi possivel converter o preço para decimal.");

        return price;
    }

    private async Task SalvarPedidoAsync(OrderModel order)
    {
        string? pedidoUUID = order.PedidoUuid;

        if (string.IsNullOrEmpty(pedidoUUID))
            throw new ArgumentNullException("O UUID do pedido está vazio ou nulo.");

        await _caching.SetCacheAsync(pedidoUUID, JsonSerializer.Serialize(order));

    }
    private async Task PostAsync(OrderModel mensagem, string uri)
    {
        // Serializa o objeto OrderModel para JSON
        string json = JsonSerializer.Serialize(mensagem);

        StringContent content = new(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync(requestUri: $"http://localhost:{_config["PORTA"]}/api/v1/{uri}", content);
    }

}