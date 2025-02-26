using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConstructionPMS.Infrastructure.Kafka
{
    public class KafkaConsumer : IKafkaConsumerService
    {
        private readonly IConsumer<Null, string> _consumer;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        public async Task ConsumeAsync(string topic, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    // Process the message
                    Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }
    }
}