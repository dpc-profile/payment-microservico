namespace Order_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger _logger;

    public OrderController(ILogger<OrderModel> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post([FromBody] OrderModel order)
    {
        switch (order.PedidoStatus)
        {
            case "Aprovado":
                // Fluxo do pedido aprovado
                break;
            case "Pendente":
                // Fluxo do pedido pendente
                break;
            case "Recusado":
                // Fluxo do pedido recusado
                break;
            case "Cancelado":
                // Fluxo do pedido cancelado
                break;
            default:
                // Fora do escopo
                // Salva o pedido no db
                // Cancelar pedido
                return BadRequest();
        }

        return Ok();
    }
}