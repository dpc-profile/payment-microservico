namespace Order_api.Services;

public class OrderServices : IOrderServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    private readonly string _produto_uri;

    public OrderServices(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;

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

    public Task AprovarPedidoAsync(OrderModel order)
    {
        throw new NotImplementedException();
    }

    public Task CancelarPedidoAsync(OrderModel order)
    {
        throw new NotImplementedException();
    }

    public Task ProcessarPagamentoAsync(OrderModel order)
    {
        throw new NotImplementedException();
    }

    public Task ReprocessarPagamentoAsync(OrderModel order)
    {
        throw new NotImplementedException();
    }

    private void ValidarJsonProduto(JsonElement produto)
    {
        if (string.IsNullOrEmpty(produto.GetProperty("product").ToString()))
            throw new ProdutoNaoValidoException("O nome do produto é nulo ou vazio");

        if (string.IsNullOrEmpty(produto.GetProperty("price").ToString()))
            throw new ProdutoNaoValidoException("O preco do produto é nulo ou vazio");
    }

    private async Task<JsonElement> ConsultarProduto(string ProdutoUuid)
    {
        string? response = await _httpClient.GetStringAsync(requestUri: $"{_produto_uri}/{ProdutoUuid}");

        using JsonDocument? jsonProduto = JsonDocument.Parse(response);

        if (jsonProduto.RootElement.ValueKind is not JsonValueKind.Object)
            throw new JsonException("Algo foi retornado da API que não é um json");

        return jsonProduto.RootElement.Clone();
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
}