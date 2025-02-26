using System;

namespace ConstructionPMS.Application.Commands
{
    public class CreateTaskCommand
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}