namespace checkout_api.Services;

public class NotifiedServices : INotifiedServices
{
    private readonly HttpClient _httpClient;

    public NotifiedServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ConsumirMensagem(OrderModel mensagem)
    {
        // Serializa o objeto OrderModel para JSON
        string? json = JsonSerializer.Serialize(mensagem);

        StringContent? content = new(content: json, encoding: Encoding.UTF8, mediaType: "application/json");

        await _httpClient.PostAsync(requestUri: "http://localhost:5221/api/v1/Checkout", content);
    }
}