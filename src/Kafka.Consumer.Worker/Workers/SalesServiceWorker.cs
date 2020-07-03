using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Consumer.Worker.Workers
{
    public class SalesServiceWorker : IHostedService, IDisposable
    {
        private readonly ILogger<SalesServiceWorker> _logger;
        private readonly IConfiguration _configuration;

        public SalesServiceWorker(ILogger<SalesServiceWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer Sales Started.....");

            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = $"{_configuration["Kafka:TopicName"]}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_configuration["Kafka:TopicName"]);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var result = consumer.Consume(cancellationToken);
                        _logger.LogInformation($"Message: {result.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                    _logger.LogWarning("Consumer run canceled.");
                }
                finally
                {
                    consumer.Close();
                }
            }

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping SalesServiceWorker...");
            return Task.CompletedTask;
        }
    }
}
