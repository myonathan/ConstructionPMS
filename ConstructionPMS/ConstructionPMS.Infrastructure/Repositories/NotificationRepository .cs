using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ConstructionDbContext _context;

        public NotificationRepository(ConstructionDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> GetByIdAsync(Guid notificationId)
        {
            return await _context.Notifications.FindAsync(notificationId);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid notificationId)
        {
            var notification = await GetByIdAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}