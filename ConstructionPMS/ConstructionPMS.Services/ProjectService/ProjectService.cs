using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Infrastructure.Repositories; // Assuming you have a repository layer
using System.ComponentModel.DataAnnotations;

namespace ConstructionPMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            // Validate the project
            project.Validate();

            // Add the project to the repository
            await _projectRepository.AddAsync(project);
            return project;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            return await _projectRepository.GetByIdAsync(projectId);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            // Validate the project
            project.Validate();

            // Update the project in the repository
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            await _projectRepository.DeleteAsync(projectId);
        }
    }
}