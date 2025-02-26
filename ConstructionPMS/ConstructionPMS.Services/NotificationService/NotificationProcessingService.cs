using System;
using System.Threading;
using ConstructionPMS.Infrastructure.Kafka;

namespace ConstructionPMS.Services.NotificationService
{
    public class NotificationProcessingService : INotificationProcessingService
    {
        private readonly KafkaConsumer _kafkaConsumer;

        public NotificationProcessingService(string bootstrapServers, string groupId)
        {
            _kafkaConsumer = new KafkaConsumer(bootstrapServers, groupId);
        }

        public async Task StartProcessing(CancellationToken cancellationToken)
        {
            // Start consuming messages from the "notifications" topic
            await _kafkaConsumer.ConsumeAsync("notifications", cancellationToken);
        }
    }
}