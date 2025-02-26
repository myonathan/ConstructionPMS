using System.Threading.Tasks;
using ConstructionPMS.Shared;
using ConstructionPMS.Shared.Exceptions;
using ConstructionPMS.Shared.Extensions;

namespace ConstructionPMS.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotificationAsync(string message)
        {
            if (message.IsNullOrEmpty())
            {
                throw new CustomException("Notification message cannot be null or empty.");
            }

            // Logic to send notification (e.g., via email, SMS, etc.)
            await Task.CompletedTask; // Placeholder for async operation
        }
    }
}