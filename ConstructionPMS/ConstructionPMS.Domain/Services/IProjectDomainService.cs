using ConstructionPMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Domain.Services
{
    public interface IProjectDomainService
    {
        Task<Project> CreateProjectAsync(string name, string description, DateTime startDate, DateTime endDate);
        Task<Project> UpdateProjectAsync(Guid projectId, string name, string description, DateTime startDate, DateTime endDate);
        Task DeleteProjectAsync(Guid projectId);
        Task<Project> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
    }
}