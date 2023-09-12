namespace checkout_api.Services;

public class CheckoutServices : ICheckoutServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    private readonly string _produto_uri;
    private readonly string _message_produce_uri;

    public CheckoutServices(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;

        _produto_uri = _config["PRODUTO:URL"];
        _produto_uri = $"{_produto_uri}/api/Produto";

        _message_produce_uri = $"http://localhost:{_config["PORTA"]}/api/MessageProducer";
    }

    public async Task ValidarProdutoAsync(string uuid)
    {
        try
        {
            string? response = await _httpClient.GetStringAsync(requestUri: $"{_produto_uri}/{uuid}");

            using JsonDocument? jsonProduto = JsonDocument.Parse(response);

            if (jsonProduto.RootElement.ValueKind is not JsonValueKind.Object)
                throw new JsonException("Algo foi retornado da API que não é um json");

            JsonElement produto = jsonProduto.RootElement;

            ValidarJsonProdutos(produto);
        }
        catch (JsonException error)
        {
            throw new ProdutoNaoValidoException(error.Message);
        }
    }

    public void ValidarUsuarioAsync(OrderModel order)
    {
        // Nenhum problema aqui, pode continuar
    }
    
    public async Task PublicarMensagemAsync(OrderModel mensagem)
    {
        // Serializa o objeto OrderModel para JSON
        string json = JsonSerializer.Serialize(mensagem);

        StringContent content = new(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync(requestUri: _message_produce_uri, content);    
    }

    private void ValidarJsonProdutos(JsonElement produto)
    {
        if (string.IsNullOrEmpty(produto.GetProperty("product").ToString())) 
            throw new ProdutoNaoValidoException("O nome do produto é nulo ou vazio");

        if (string.IsNullOrEmpty(produto.GetProperty("price").ToString())) 
            throw new ProdutoNaoValidoException("O preco do produto é nulo ou vazio");
    }
}