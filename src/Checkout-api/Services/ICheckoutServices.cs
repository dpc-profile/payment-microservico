namespace checkout_api.Services;

public interface ICheckoutServices
{
    public Task ValidarProdutoAsync(string uuid);
    public void ValidarUsuarioAsync(OrderModel order);
    public Task PublicarMensagemAsync(OrderModel order);
}