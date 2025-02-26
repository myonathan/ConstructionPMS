using System.Threading.Tasks;

namespace ConstructionPMS.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string message);
    }
}