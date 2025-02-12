using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskEngine.Domain.Entities.Settings;

namespace TaskEngine.Application.Workers
{
    public class JobWorker : BackgroundService
    {
        private readonly IModel _channel;
        private readonly HttpClient _httpClient;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ServiceSettings _serviceSettings;

        public JobWorker(IModel channel, HttpClient httpClient, IOptions<RabbitMQSettings> rabbitMQSettings, IOptions<ServiceSettings> serviceSettings)
        {
            _channel = channel;
            _httpClient = httpClient;
            _rabbitMQSettings = rabbitMQSettings.Value;
            _serviceSettings = serviceSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queues = await GetQueueToProcess();

            foreach (var queue in queues)
            {
                await ConsumeQueue(queue, stoppingToken);
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task ConsumeQueue(string queue, CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{queue}] Message received: {message}");

                try
                {
                    await ProcessMessage(queue,message, stoppingToken);

                    _channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"[{queue}] Process done.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{queue}] Error in process: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private async Task ProcessMessage(string queue, string message, CancellationToken stoppingToken)
        {
            Console.WriteLine($"[{queue}] Processing message: {message}");

            var apiUrl = _serviceSettings.BaseUrl;
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_serviceSettings.BaseUrl}/api/Job/{queue}", content, stoppingToken);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine($"[{queue}] API call successful for message: {message}");
                else
                {
                    Console.WriteLine($"[{queue}] API call failed for message: {message}. Status Code: {response.StatusCode}");
                    throw new Exception($"[{queue}] Error: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{queue}] Error calling API for message: {message}. Exception: {ex.Message}");
                throw ex;
            }
        }

        private Task<List<string>> GetQueueToProcess()
        {
            return Task.FromResult(_rabbitMQSettings.Queues);
        }
    }
}
