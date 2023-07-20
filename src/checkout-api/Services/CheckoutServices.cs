namespace checkout_api.Services;

public class CheckoutServices : ICheckoutServices
{
    public Task<ProdutoModel> ConsultarProdutoAPIAsync(string uuid)
    {
        throw new NotImplementedException();
    }

    public OrderModel ConsumirMensagem()
    {
        throw new NotImplementedException();
    }

    public Task PublicarMensagemAsync(OrderMessageModel message)
    {
        throw new NotImplementedException();
    }
}