var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<ICachingServices, CachingServices>();

// Permite que o MessageConsumer fique rodando em "loop infinito" procurando mensagem
builder.Services.AddHostedService<MessageConsumer>();

builder.Services.AddStackExchangeRedisCache(o => {
    o.InstanceName = config["REDIS:INSTANCE"] ;
    o.Configuration = $"{config["REDIS:HOST"]}:{config["REDIS:PORTA"]}";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
