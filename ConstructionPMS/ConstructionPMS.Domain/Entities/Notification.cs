using System;

namespace ConstructionPMS.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; private set; }

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