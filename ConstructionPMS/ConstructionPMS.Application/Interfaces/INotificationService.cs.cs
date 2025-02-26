using ConstructionPMS.Application.Commands;
using ConstructionPMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Application.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationCommand command);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserQueryIdAsync(Guid userId);
        Task DeleteNotificationAsync(Guid notificationId);
    }
}