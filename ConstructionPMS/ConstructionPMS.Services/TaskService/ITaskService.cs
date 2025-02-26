using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<ProjectTask>> GetAllTasksAsync();
        Task<ProjectTask> GetTaskByIdAsync(Guid taskId);
        Task<ProjectTask> CreateTaskAsync(ProjectTask task);
        Task UpdateTaskAsync(ProjectTask task);
        Task DeleteTaskAsync(Guid taskId);
    }
}