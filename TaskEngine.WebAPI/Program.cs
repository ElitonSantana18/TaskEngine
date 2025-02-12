using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using TaskEngine.Domain.Entities.Settings;
using TaskEngine.WebAPI;
using TaskEngine.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// MongoDB Settings Configure
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// RabbitMQ Settings Configure
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

// Service Settings Configure
builder.Services.Configure<ServiceSettings>(
    builder.Configuration.GetSection("Service"));

ServiceCollectionExtension.ConfigureServices(builder);
ServiceCollectionExtension.ConfigureRepositorys(builder);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// Add HttpClient
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Configurar para a aplicação escutar em todas as interfaces de rede (0.0.0.0) na porta 5000 (para http)
app.Urls.Add("http://0.0.0.0:5000");

app.Run();
