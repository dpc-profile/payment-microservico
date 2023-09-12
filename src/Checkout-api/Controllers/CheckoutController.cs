namespace checkout_api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            if (string.IsNullOrEmpty(order.ProdutoUuid)) throw new ProdutoNaoValidoException("O UUID do produto é nulo.");

            // Validar se o produto é valido
            await _checkoutServices.ValidarProdutoAsync(order.ProdutoUuid);

            // Validar dados do usuário
            _checkoutServices.ValidarUsuarioAsync(order);

            // Posta a mensagem na fila order_ex
            await _checkoutServices.PublicarMensagemAsync(order);

            return Ok();
        }
        catch (HttpRequestException error)
        {
            _logger.LogWarning(message: error.Message, args: error);
            return NotFound("O produto procurado não existe.");
        }
        catch (ProdutoNaoValidoException error)
        {
            _logger.LogWarning(message: "O produto não é válido", args: error.Message);
            return BadRequest("O produto não é válido");
        }
        catch (UsuarioNaoValidoException error)
        {
            _logger.LogWarning(message: "Os dados do usuário não é válido", args: error.Message);
            return BadRequest("Os dados do usuário não é válido");
        }
        catch (Exception error)
        {
            _logger.LogError(message: $"Erro generico em {nameof(PopulaMensagemComPedido)}: ", args: error.Message);
            return BadRequest("Alguem tropeço e derrubou o pedido, acabou explodiu sobrando nada.");
        }
    }
}