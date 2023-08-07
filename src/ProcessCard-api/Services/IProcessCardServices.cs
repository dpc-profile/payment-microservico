namespace ProcessCard_api.Services;

public interface IProcessCardServices
{
    public void FazerCobranca(OrderModel order);
    public Task PostAsync(OrderModel mensagem, string uri);
}
