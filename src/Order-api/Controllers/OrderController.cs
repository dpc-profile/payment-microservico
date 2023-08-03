namespace Order_api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IOrderServices _orderServices;

    public OrderController(ILogger<OrderController> logger, IOrderServices orderServices)
    {
        _logger = logger;
        _orderServices = orderServices;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessarPedidoAsync([FromBody] OrderModel order)
    {
        try
        {
            await _orderServices.ValidarEAtualizarPedidoAsync(order);

            switch (order.PedidoStatus)
            {
                case "Aprovado":
                    await _orderServices.AprovarPedidoAsync(order);
                    break;

                case "Pendente":
                    await _orderServices.ProcessarPagamentoAsync(order);
                    break;

                case "Recusado":
                    await _orderServices.ReprocessarPagamentoAsync(order);
                    break;

                case "Cancelado":
                    await _orderServices.CancelarPedidoAsync(order);
                    break;
            }

            return Ok();
        }
        catch (HttpRequestException error)
        {
            _logger.LogWarning(message: error.Message, args: error);
            return NotFound("Não foi possivel achar o produto procurado.");
        }
        catch (ProdutoNaoValidoException error)
        {
            _logger.LogWarning(message: "O produto não é válido", args: error.Message);
            return BadRequest("O produto não é válido");
        }
        catch (FormatException error)
        {
            _logger.LogWarning(message: error.Message, args: error);
            return BadRequest("Não foi possivel converter o preço do produto.");
        }
        catch (ArgumentNullException error)
        {
            _logger.LogError(error.Message, error);
            return BadRequest("Algo de errado com o pedido ou o produto.");
        }
        catch (Exception error)
        {
            _logger.LogError(message: $"Erro não mapeado em {nameof(ProcessarPedidoAsync)}: ", args: error);
            return BadRequest("Alguem tropeço e derrubou o pedido, acabou explodiu sobrando nada.");
        }
    }

}