using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using System.Threading.Channels;
using TaskEngine.Application.Workers;
using TaskEngine.Domain.Entities.Settings;
using TaskEngine.Domain.Interfaces.Repository;
using TaskEngine.Domain.Interfaces.Service;
using TaskEngine.Domain.Interfaces.Settings;
using TaskEngine.Domain.Service;
using TaskEngine.Infrastructure.Repository;

namespace TaskEngine.WebAPI
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Application Services 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            ConfigureWorker(builder);
            builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            builder.Services.AddScoped<IJobService, JobService>();
        }

        private static void ConfigureWorker(WebApplicationBuilder builder)
        {
            var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            //Configurar RabbitMQ e injetar IModel
            builder.Services.AddSingleton<IConnection>(provider =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitMQSettings.HostName,
                    Port = 5672,
                    UserName = rabbitMQSettings.UserName,
                    Password = rabbitMQSettings.Password,
                    DispatchConsumersAsync = true
                };

                return factory.CreateConnection();
            });

            builder.Services.AddSingleton<IModel>(provider =>
            {
                var connection = provider.GetRequiredService<IConnection>();
                var channel = connection.CreateModel();
                channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

                foreach (var queue in rabbitMQSettings.Queues)
                {
                    channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    Console.WriteLine($"[RabbitMQ] Created Queue: {queue}");
                }

                return channel;
            });

            //Adding Worker
            builder.Services.AddHostedService<JobWorker>();
        }

        /// <summary>
        /// Repositorys Application
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepositorys(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddSingleton<IJobRepository, JobRepository>();
        }
    }
}
