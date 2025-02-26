using System.Threading;
using System.Threading.Tasks;

namespace ConstructionPMS.Infrastructure.Kafka
{
    public interface IKafkaConsumerService
    {
        Task ConsumeAsync(string topic, CancellationToken cancellationToken);
    }
}