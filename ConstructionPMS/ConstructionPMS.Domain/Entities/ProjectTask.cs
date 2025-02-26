using System;

namespace ConstructionPMS.Domain.Entities
{
    public class ProjectTask
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public TaskStatus Status { get; private set; }
        public DateTime DueDate { get; private set; }
        public Guid ProjectId { get; private set; }

        public ProjectTask(string title, TaskStatus status, DateTime dueDate, Guid projectId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Status = status;
            DueDate = dueDate;
            ProjectId = projectId;
        }

        // Additional methods for business logic can be added here
    }
}