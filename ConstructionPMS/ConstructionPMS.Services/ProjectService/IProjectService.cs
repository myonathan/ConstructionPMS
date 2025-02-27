using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;

namespace ConstructionPMS.Services
{
    public interface IProjectService
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int projectId);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int projectId);
    }
}