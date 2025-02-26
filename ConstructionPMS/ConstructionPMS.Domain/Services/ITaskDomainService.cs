using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Domain.Services
{
    public interface ITaskDomainService
    {
        Task<ProjectTask> CreateTaskAsync(string title, Entities.TaskStatus status, DateTime dueDate, Guid projectId);
        Task<ProjectTask> UpdateTaskAsync(Guid taskId, string title, Entities.TaskStatus status, DateTime dueDate);
        Task DeleteTaskAsync(Guid taskId);
        Task<ProjectTask> GetTaskByIdAsync(Guid taskId);
        Task<IEnumerable<ProjectTask>> GetAllTasksAsync();
    }
}