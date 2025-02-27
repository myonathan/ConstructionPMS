using System;

namespace ConstructionPMS.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }

        public Notification(string message, Guid userId)
        {
            Id = Guid.NewGuid();
            Message = message;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
        }

        // Additional methods for business logic can be added here
    }
}