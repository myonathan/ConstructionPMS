using ConstructionPMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Domain.Services
{
    public interface ITaskDomainService
    {
        Task<ProjectTask> CreateTaskAsync(string title, TaskStatus status, DateTime dueDate, Guid projectId);
        Task<ProjectTask> UpdateTaskAsync(Guid taskId, string title, TaskStatus status, DateTime dueDate);
        Task DeleteTaskAsync(Guid taskId);
        Task<ProjectTask> GetTaskByIdAsync(Guid taskId);
        Task<IEnumerable<ProjectTask>> GetAllTasksAsync();
    }
}