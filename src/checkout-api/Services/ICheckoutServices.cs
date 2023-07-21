namespace checkout_api.Services;

public interface ICheckoutServices
{
    public Task<ProdutoModel> ConsultarProduto(string uuid);

    public OrderMessageModel CriarOrderMessage(ProdutoModel dadosProduto, OrderModel dadosUsuario);

    public void PublicarMensagem(OrderMessageModel mensagem);
}