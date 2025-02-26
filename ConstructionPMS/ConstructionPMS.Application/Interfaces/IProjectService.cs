using ConstructionPMS.Application.Commands;
using ConstructionPMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(CreateProjectCommand command);
        Task<ProjectDto> UpdateProjectAsync(UpdateProjectCommand command);
        Task DeleteProjectAsync(DeleteProjectCommand command);
        Task<ProjectDto> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    }
}