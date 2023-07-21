namespace checkout_api.Services;

public interface INotifiedServices
{
    public Task ConsumirMensagem(OrderModel mensagem);

}