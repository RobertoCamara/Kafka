using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult SendMessage([FromQuery] string msg)
        {
            return Created("", SendMessageByKafka(msg));
        }

        private string SendMessageByKafka(string message)
        {
            var config = new ProducerConfig { BootstrapServers = _configuration["Kafka:BootstrapServers"] };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = producer
                                        .ProduceAsync(_configuration["Kafka:TopicName"], new Message<Null, string> { Value = message })
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
}
