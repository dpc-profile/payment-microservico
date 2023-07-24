using RabbitMQ.Client;

namespace client_app.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ConnectionFactory _factory;
    private readonly IConfiguration _config;
    private readonly string _checkout_exchange;

    public MessageController(IConfiguration configuration)
    {
        _config = configuration;
        _checkout_exchange = _config["RABBITMQ:EX_PRODUCE"];
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
                channel.ExchangeDeclare(
                    exchange: _checkout_exchange,
                    type: ExchangeType.Direct
                );

                byte[] bytesMessage = JsonSerializer.SerializeToUtf8Bytes(message);

                channel.BasicPublish(
                    exchange: _checkout_exchange,
                    routingKey: "",
                    basicProperties: null,
                    body: bytesMessage
                );
            }
        }

        return Accepted();
    }

}