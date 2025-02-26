using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(Guid projectId);
        Task<Project> AddAsync(Project project); // Ensure this returns Project
        Task UpdateAsync(Project project);
        Task DeleteAsync(Guid projectId);
    }
}