using ConstructionPMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Domain.Services
{
    public interface INotificationDomainService
    {
        Task<Notification> CreateNotificationAsync(string message, Guid userId);
        Task DeleteNotificationAsync(Guid notificationId);
        Task<Notification> GetNotificationByIdAsync(Guid notificationId);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    }
}