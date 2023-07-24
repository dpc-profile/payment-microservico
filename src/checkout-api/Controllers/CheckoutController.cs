namespace checkout_api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ICheckoutServices _checkoutServices;

    public CheckoutController(ILogger<CheckoutController> logger, ICheckoutServices checkoutServices)
    {
        _logger = logger;
        _checkoutServices = checkoutServices;
    }

    // Recebe o POST de MessageConsumer
    [HttpPost]
    public async Task<IActionResult> PopulaMensagemComPedido([FromBody] OrderModel order)
    {
        try
        {
            if (order.ProdutoUuid is null) throw new KeyNotFoundException("O UUID do produto é nulo.");

            // Consultar o produto-api para coletar os dados do pedido
            ProdutoModel? produto = await _checkoutServices.ConsultarProdutoAsync(order.ProdutoUuid);

            // Junta os dados do pedido com os dados do usuario
            OrderMessageModel mensagem = _checkoutServices.CriarOrderMessage(dadosProduto: produto, dadosUsuario: order);

            // Posta a mensagem na fila order_ex
            await _checkoutServices.PublicarMensagemAsync(mensagem);

            return Ok();
        }
        catch (KeyNotFoundException error)
        {
            _logger.LogError(message: error.Message, args: error);
            return NotFound("O UUID do produto é nulo.");
        }
        catch (NotFoundException error)
        {
            _logger.LogError(message: error.Message, args: error);
            return NotFound("O produto procurado não existe.");
        }
        catch (Exception error)
        {
            _logger.LogError(message: $"Erro generico em {nameof(PopulaMensagemComPedido)}: ", args: error.Message);
            return BadRequest(error: "O processo do pedido explodiu por acidente.");
        }
    }
}