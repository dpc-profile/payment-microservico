using RabbitMQ.Client;

namespace client_app.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageProducerController : ControllerBase
{
    private readonly ConnectionFactory _factory;
    private readonly IConfiguration _config;
    private readonly string _exchange;

    public MessageProducerController(IConfiguration config)
    {
        _config = config;
        _exchange = _config["RABBITMQ:EX_PRODUCE"];
        _factory = new ConnectionFactory
        {
            HostName = _config["RABBITMQ:HOST"],
            UserName = CryptoHelper.Decrypt(encryptText: _config["RABBITMQ:USERNAME"], decryptKey: _config["RABBITMQ:KEY"]),
            Password = CryptoHelper.Decrypt(encryptText: _config["RABBITMQ:PASSWORD"], decryptKey: _config["RABBITMQ:KEY"]),
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
                    exchange: _exchange,
                    type: ExchangeType.Direct,
                    durable: true,
                    autoDelete: false,
                    arguments: null
                );

                byte[] bytesMessage = JsonSerializer.SerializeToUtf8Bytes(message);

                channel.BasicPublish(
                    exchange: _exchange,
                    routingKey: "",
                    basicProperties: null,
                    body: bytesMessage
                );
            }
        }

        return Accepted();
    }

}