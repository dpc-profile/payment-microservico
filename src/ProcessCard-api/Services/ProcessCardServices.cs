using System.Collections;

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
        // Essa função vai aprovar, reprovar ou cancelar
        // um pedido de forma aleatória, apenas para
        // estressar o fluxo do processo ao todo
        // ou por conta de informações

        if (string.IsNullOrEmpty(order.UsuarioNome))
        {
            order.PedidoStatus = "Cancelado";
            order.UpdatedAt = DateTime.Now;
            return;
        }

        Random? rand = new();

        // random entre 0 e 50
        int randInt = rand.Next(51);

        int intIncrementado = randInt + order.UsuarioNome.Length - (int)Math.Round(order.ProdutoPreco);

        // Converte em porcentage
        int chances = intIncrementado * 100 / 50;
        
        switch(chances){
            case >= 60:
                order.PedidoStatus = "Aprovado";
                order.UpdatedAt = DateTime.Now;
                return;
            case >= 20:
                order.PedidoStatus = "Recusado";
                order.UpdatedAt = DateTime.Now;
                return;
            default:
                order.PedidoStatus = "Cancelado";
                order.UpdatedAt = DateTime.Now;
                return;
        }
    }

    public async Task PostAsync(OrderModel mensagem, string uri)
    {
        // Serializa o objeto OrderModel para JSON
        string json = JsonSerializer.Serialize(mensagem);

        StringContent content = new(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync(requestUri: $"http://localhost:{_config["PORTA"]}/api/{uri}", content);
    }
}
