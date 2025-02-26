using System.Threading.Tasks;

namespace ConstructionPMS.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string message);
    }
}