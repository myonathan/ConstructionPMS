using ConstructionPMS.Application.Commands;
using ConstructionPMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Application.Interfaces
{
    public interface ITaskService
    {
        Task<ProjectTaskDto> CreateTaskAsync(CreateTaskCommand command);
        Task<ProjectTaskDto> UpdateTaskAsync(UpdateTaskCommand command);
        Task DeleteTaskAsync(DeleteTaskCommand command);
        Task<ProjectTaskDto> GetTaskByIdAsync(Guid taskId);
        Task<IEnumerable<ProjectTaskDto>> GetAllTasksAsync();
    }
}