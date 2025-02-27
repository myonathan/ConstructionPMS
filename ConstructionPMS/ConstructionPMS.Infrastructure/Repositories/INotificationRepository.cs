using ConstructionPMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(Guid notificationId);
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Notification notification);
        Task DeleteAsync(Guid notificationId);
    }
}