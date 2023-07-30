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
    public string Post([FromBody] OrderModel order)
    {
        string json = JsonSerializer.Serialize(order);
        return json;
    }
}