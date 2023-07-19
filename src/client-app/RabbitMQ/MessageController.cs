using RabbitMQ.Client;

namespace client_app.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ConnectionFactory _factory;
    private readonly IConfiguration _config;
    private readonly string checkout_queue;

    public MessageController(IConfiguration configuration)
    {
        _config = configuration;
        checkout_queue = _config["RABBITMQ:QUEUE"];
        _factory = new ConnectionFactory
        {
            HostName = _config["RABBITMQ:HOST"],
            UserName = _config["RABBITMQ:USERNAME"],
            Password = _config["RABBITMQ:PASSWORD"],
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
                    queue: checkout_queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                byte[] bytesMessage = JsonSerializer.SerializeToUtf8Bytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: checkout_queue,
                    basicProperties: null,
                    body: bytesMessage
                );
            }
        }

        return Accepted();
    }

}