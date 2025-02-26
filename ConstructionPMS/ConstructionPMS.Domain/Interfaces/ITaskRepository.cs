using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<ProjectTask>> GetAllAsync();
        Task<ProjectTask> GetByIdAsync(Guid taskId);
        Task<ProjectTask> AddAsync(ProjectTask task); // Ensure this returns ProjectTask
        Task UpdateAsync(ProjectTask task);
        Task DeleteAsync(Guid taskId);
    }
}