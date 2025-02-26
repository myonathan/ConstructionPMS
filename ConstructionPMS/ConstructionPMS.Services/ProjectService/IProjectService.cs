using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(Guid projectId);
        Task<Project> CreateProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Guid projectId);
    }
}