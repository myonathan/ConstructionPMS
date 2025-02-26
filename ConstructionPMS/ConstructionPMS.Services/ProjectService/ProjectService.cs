using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Domain.Interfaces;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Shared.Exceptions;

namespace ConstructionPMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new CustomException($"Project with ID {projectId} not found.");
            }
            return project;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            if (project == null)
            {
                throw new CustomException("Project cannot be null.");
            }
            return await _projectRepository.AddAsync(project);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            if (project == null)
            {
                throw new CustomException("Project cannot be null.");
            }
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            await _projectRepository.DeleteAsync(projectId);
        }
    }
}