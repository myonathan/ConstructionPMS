using System.Threading;

namespace ConstructionPMS.Services.NotificationService
{
    public interface INotificationProcessingService
    {
        Task StartProcessing(CancellationToken cancellationToken);
    }
}