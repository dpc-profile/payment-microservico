public interface ICheckoutServices
{
    public OrderModel ConsumirMensagem();
    public Task<ProdutoModel> ConsultarProdutoAPIAsync(string uuid);

    public Task PublicarMensagemAsync(OrderMessageModel message);

}