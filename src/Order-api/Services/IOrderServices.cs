namespace Order_api.Services;
public interface IOrderServices
{
    public Task ValidarEAtualizarPedidoAsync(OrderModel order);
    public Task AprovarPedidoAsync(OrderModel order);
    public Task CancelarPedidoAsync(OrderModel order);
    public Task ProcessarPagamentoAsync(OrderModel order);
    public Task ReprocessarPagamentoAsync(OrderModel order);
    public Task<string> PegarPedido(string pedidoUUID);
}