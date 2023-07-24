namespace checkout_api.RabbitMQ;

public class MessageConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;
    private readonly IModel _channel;

    private readonly string _queue;

    public MessageConsumer(IServiceProvider serviceProvider, IConfiguration config)
    {
        _serviceProvider = serviceProvider;
        _config = config;

        ConnectionFactory? factory = new()
        {
            HostName = _config["RABBITMQ:HOST"],
            UserName = _config["RABBITMQ:USERNAME"],
            Password = _config["RABBITMQ:PASSWORD"],
        };

        _queue = _config["RABBITMQ:QUEUE_CONSUME"];

        IConnection _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: _queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        EventingBasicConsumer? consumer = new(_channel);

        consumer.Received += (sender, eventArgs) =>
        {
            byte[] contentArray = eventArgs.Body.ToArray();
            Utf8JsonReader utf8Content = new(contentArray);

            OrderModel? mensagem = JsonSerializer.Deserialize<OrderModel>(ref utf8Content);

            ConsumirMensagemAsync(mensagem);

            _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: _queue, autoAck: false, consumer);

        return Task.CompletedTask;
    }

    private async void ConsumirMensagemAsync(OrderModel mensagem)
    {
        // Serializa o objeto OrderModel para JSON
        string? json = JsonSerializer.Serialize(mensagem);

        StringContent? content = new(content: json, encoding: Encoding.UTF8, mediaType: "application/json");

        HttpClient client = new();
        await client.PostAsync(requestUri: "http://localhost:5221/api/v1/Checkout", content);
    }
}