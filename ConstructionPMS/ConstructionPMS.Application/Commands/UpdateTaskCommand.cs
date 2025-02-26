using System;

namespace ConstructionPMS.Application.Commands
{
    public class UpdateTaskCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}