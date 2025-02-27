using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using Nest;

namespace ConstructionPMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IElasticClient _elasticClient; // Elasticsearch client

        public ProjectService(IProjectRepository projectRepository, IElasticClient elasticClient)
        {
            _projectRepository = projectRepository;
            _elasticClient = elasticClient;
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

        public async Task<IEnumerable<Project>> GetAllProjectsFromElasticSearchAsync()
        {
            // Define the search request
            var searchResponse = await _elasticClient.SearchAsync<Project>(s => s
                .Index("projects") // The index name in Elasticsearch
                .From(0) // Starting from the first record
                .Size(1000) // Limit the number of records returned
                .Sort(so => so
                    .Ascending(p => p.ProjectId) // Sort by ProjectId
                )
            );

            // Check for errors
            if (!searchResponse.IsValid)
            {
                throw new Exception($"Elasticsearch query failed: {searchResponse.OriginalException.Message}");
            }

            // Return the list of projects
            return searchResponse.Documents;
        }

        public async Task<Project> GetProjectByIdFromElasticSearchAsync(int id)
        {
            // Define the search request to find the project by ID
            var searchResponse = await _elasticClient.SearchAsync<Project>(s => s
                .Index("projects") // The index name in Elasticsearch
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.ProjectId) // Assuming ProjectId is the field name in Elasticsearch
                        .Value(id) // The ID to search for
                    )
                )
            );

            // Check for errors
            if (!searchResponse.IsValid)
            {
                throw new Exception($"Elasticsearch query failed: {searchResponse.OriginalException.Message}");
            }

            // Return the first matching project or null if not found
            return searchResponse.Documents.FirstOrDefault();
        }
    }
}