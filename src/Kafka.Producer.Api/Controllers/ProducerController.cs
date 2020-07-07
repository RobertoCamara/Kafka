using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kafka.Producer.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProducerController: ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IConfiguration _configuration;

        public ProducerController(ILogger<ProducerController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("SendMessage")]
        public IActionResult SendMessage([FromBody] Sale sale)
        {
            return Created("", SendMessageByKafka(sale));
        }

        private string SendMessageByKafka(Sale message)
        {
            var config = new ProducerConfig { BootstrapServers = _configuration["Kafka:BootstrapServers"] };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    string data = JsonConvert.SerializeObject(message);
                    var result = producer
                                        .ProduceAsync(_configuration["Kafka:TopicName"], new Message<Null, string> { Value = data })
                                        .GetAwaiter()
                                        .GetResult();

                    return $"Message '{result.Value}' of '{result.TopicPartitionOffset}'";
                }
                catch (ProduceException<Null, string> e)
                {
                    _logger.LogError($"Delivery failed: {e.Error.Reason}");
                }
            }

            return string.Empty;
        }
    }

    public class Sale
    {
        public string User { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }

    
}
