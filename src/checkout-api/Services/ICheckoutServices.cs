namespace checkout_api.Services;

public interface ICheckoutServices
{
    public Task<ProdutoModel> ConsultarProdutoAsync(string uuid);

    public OrderMessageModel CriarOrderMessage(ProdutoModel dadosProduto, OrderModel dadosOrder);

    public Task PublicarMensagemAsync(OrderMessageModel mensagem);
}