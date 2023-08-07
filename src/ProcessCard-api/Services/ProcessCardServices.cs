namespace ProcessCard_api.Services;

public class ProcessCardServices : IProcessCardServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ProcessCardServices(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public void FazerCobranca(OrderModel order)
    {
        order.PedidoStatus = "Aprovado";
    }

    public async Task PostAsync(OrderModel mensagem, string uri)
    {
        // Serializa o objeto OrderModel para JSON
        string json = JsonSerializer.Serialize(mensagem);

        StringContent content = new(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync(requestUri: $"http://localhost:{_config["PORTA"]}/api/v1/{uri}", content);
    }
}
