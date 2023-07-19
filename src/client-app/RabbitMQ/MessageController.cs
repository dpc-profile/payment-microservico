using RabbitMQ.Client;

namespace client_app.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ConnectionFactory _factory;
    private readonly IConfiguration _configuration;
    private const string CHECKOUT_QUEUE = "checkout_ex";

    public MessageController(IConfiguration configuration)
    {
        _configuration = configuration;
        _factory = new ConnectionFactory
        {
            HostName = _configuration["RABBITMQ_HOSTNAME"],
            UserName = _configuration["RABBITMQ_USERNAME"],
            Password = _configuration["RABBITMQ_PASSWORD"]
        };
    }

    [HttpPost]
    public IActionResult PostMessage([FromBody] OrderModel message)
    {
        using (IConnection? connection = _factory.CreateConnection())
        {
            using (IModel? channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: CHECKOUT_QUEUE,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                byte[] bytesMessage = JsonSerializer.SerializeToUtf8Bytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: CHECKOUT_QUEUE,
                    basicProperties: null,
                    body: bytesMessage
                );
            }
        }

        return Accepted();
    }

}