using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Producer.Worker.Workers
{
    public class PaymentServiceWorker : IHostedService, IDisposable
    {
        private readonly ILogger<PaymentServiceWorker> _logger;
        private readonly IConfiguration _configuration;

        public PaymentServiceWorker(ILogger<PaymentServiceWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //var config = new ProducerConfig
            //{
            //    BootstrapServers = _configuration["Kafka:BootstrapServers"],
            //    ClientId = Dns.GetHostName(),
            //};

            //using (var producer = new ProducerBuilder<Null, string>(config).Build())
            //{

            //}

            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //        await Task.Delay(1000, cancellationToken);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex.Message);
            //    }
            //}

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping PaymentServiceWorker...");
            return Task.CompletedTask;
        }
    }
}
