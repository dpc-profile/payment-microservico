namespace checkout_api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CheckoutController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        return new string[] { "Ola1", "ola2" };
    }

    [HttpGet("{id}")]
    public ActionResult<string> Get(string id)
    {
        return "bom dia";
    }

    [HttpPost]
    public void FinalizarCompra([FromBody] string value)
    {
        
    }
}