using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ConstructionPMS.Shared;

namespace ConstructionPMS.Infrastructure.Kafka
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private IConsumer<string, string> _consumer;

        public KafkaConsumerBackgroundService(IConfiguration configuration, ILogger<KafkaConsumerBackgroundService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_configuration["Kafka:Topic"]);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    // Process the message
                    _logger.LogInformation($"Consumed message '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error occurred: {e.Error.Reason}");
                }
            }

            _consumer.Close();
        }
    }
}