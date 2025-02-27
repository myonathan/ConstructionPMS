using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Infrastructure.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(int projectId);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int projectId);
    }
}