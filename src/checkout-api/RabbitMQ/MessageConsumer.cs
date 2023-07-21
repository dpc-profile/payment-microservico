namespace checkout_api.RabbitMQ;

public class MessageConsumer : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    private readonly string _checkout_queue;

    public MessageConsumer(IServiceProvider serviceProvider, IConfiguration config, ILogger<MessageConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _config = config;
        _logger = logger;

        ConnectionFactory? factory = new()
        {
            HostName = _config["RABBITMQ:HOST"],
            UserName = _config["RABBITMQ:USERNAME"],
            Password = _config["RABBITMQ:PASSWORD"],
        };

        _checkout_queue = _config["RABBITMQ:QUEUE_CONSUME"];

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: _checkout_queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        EventingBasicConsumer? consumer = new(model: _channel);

        consumer.Received += (sender, eventArgs) =>
        {
            byte[] contentArray = eventArgs.Body.ToArray();
            Utf8JsonReader utf8Content = new(contentArray);

            OrderModel? mensagem = JsonSerializer.Deserialize<OrderModel>(ref utf8Content);

            NotifyUser(mensagem);

            _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: _checkout_queue, autoAck: false, consumer);

        return Task.CompletedTask;
    }

    public void NotifyUser(OrderModel mensagem)
    {
        using (IServiceScope? scope = _serviceProvider.CreateScope())
        {
            INotifiedServices? notificationService = scope.ServiceProvider.GetRequiredService<INotifiedServices>();

            notificationService.ConsumirMensagem(mensagem);
        }
    }
}