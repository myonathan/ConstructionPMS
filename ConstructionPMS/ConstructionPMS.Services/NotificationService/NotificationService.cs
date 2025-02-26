using System;
using System.Threading.Tasks;
using ConstructionPMS.Infrastructure.Kafka;
using ConstructionPMS.Services.NotificationService;

namespace ConstructionPMS.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IKafkaProducerService _kafkaProducer;

        public NotificationService(IKafkaProducerService kafkaProducer)
        {
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        }

        public async Task SendNotificationAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            // Send the notification message to the Kafka topic
            await _kafkaProducer.ProduceAsync("notifications", message);
        }
    }
}