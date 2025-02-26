using System.Threading.Tasks;

namespace ConstructionPMS.Infrastructure.Kafka
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync(string topic, string message);
    }
}