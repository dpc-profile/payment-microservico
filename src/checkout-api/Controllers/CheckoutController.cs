namespace checkout_api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class CheckoutController : ControllerBase
{
    public IActionResult ValidaPedido()
    {
        // Consumir a mensagem do pedido

        // Consultar o produto-api para coletar os dados do pedido

        // Junta os dados do pedido com os dados do usuario

        // Posta a mensagem na fila order_ex

        return Ok();
    }

    // [HttpGet]
    // public ActionResult<IEnumerable<string>> Get()
    // {
    //     return new string[] { "Ola1", "ola2" };
    // }
}