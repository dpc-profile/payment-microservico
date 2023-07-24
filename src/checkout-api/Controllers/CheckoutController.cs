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
            // Consultar o produto-api para coletar os dados do pedido
            ProdutoModel? produto = await _checkoutServices.ConsultarProdutoAsync(order.ProdutoUuid);

            // Junta os dados do pedido com os dados do usuario
            OrderMessageModel mensagem = _checkoutServices.CriarOrderMessage(dadosProduto: produto, dadosUsuario: order);

            // Posta a mensagem na fila order_ex
            await _checkoutServices.PublicarMensagemAsync(mensagem);

            return Ok();
        }
        catch (NotFoundException error)
        {
            _logger.LogError(message: "O produto procurado não existe.", args: error.Message);
            return NotFound("O produto procurado não existe.");
        }
        catch (Exception error)
        {
            _logger.LogError(message: $"Erro generico em {nameof(PopulaMensagemComPedido)}: ", args: error.Message);
            return BadRequest(error: "O processo do pedido explodiu por acidente.");
        }
    }
}