using System;

namespace ConstructionPMS.Application.Commands
{
    public class CreateNotificationCommand
    {
        public string Message { get; set; }
        public Guid UserId { get; set; }
    }
}