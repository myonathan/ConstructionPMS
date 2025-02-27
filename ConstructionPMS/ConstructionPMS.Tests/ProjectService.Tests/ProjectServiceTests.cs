using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Nest;
using Xunit;

namespace ConstructionPMS.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _elasticClientMock = new Mock<IElasticClient>();
            _projectService = new ProjectService(_configurationMock.Object, _projectRepositoryMock.Object, _elasticClientMock.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldAddProject()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "Test Project", ConstructionStartDate = DateTime.Now.AddDays(1) };
            _projectRepositoryMock.Setup(repo => repo.AddAsync(project)).Returns(Task.CompletedTask);

            // Act
            var result = await _projectService.CreateProjectAsync(project);

            // Assert
            Assert.Equal(project, result);
            _projectRepositoryMock.Verify(repo => repo.AddAsync(project), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { ProjectId = 1, ProjectName = "Project 1" },
                new Project { ProjectId = 2, ProjectName = "Project 2" }
            };
            _projectRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projects);

            // Act
            var result = await _projectService.GetAllProjectsAsync();

            // Assert
            Assert.Equal(projects, result);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnProject()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "Test Project" };
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(project);

            // Act
            var result = await _projectService.GetProjectByIdAsync(1);

            // Assert
            Assert.Equal(project, result);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldUpdateProject()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "Updated Project", ConstructionStartDate = DateTime.Now.AddDays(1) };
            _projectRepositoryMock.Setup(repo => repo.UpdateAsync(project)).Returns(Task.CompletedTask);

            // Act
            await _projectService.UpdateProjectAsync(project);

            // Assert
            _projectRepositoryMock.Verify(repo => repo.UpdateAsync(project), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldDeleteProject()
        {
            // Arrange
            var projectId = 1;
            _projectRepositoryMock.Setup(repo => repo.DeleteAsync(projectId)).Returns(Task.CompletedTask);

            // Act
            await _projectService.DeleteProjectAsync(projectId);

            // Assert
            _projectRepositoryMock.Verify(repo => repo.DeleteAsync(projectId), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectsFromElasticSearchAsync_ShouldReturnProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { ProjectId = 1, ProjectName = "Project 1" },
                new Project { ProjectId = 2, ProjectName = "Project 2" }
            };

            var searchResponse = new Mock<ISearchResponse<Project>>();
            searchResponse.Setup(s => s.Documents).Returns(projects);
            searchResponse.Setup(s => s.IsValid).Returns(true);

            _elasticClientMock.Setup(client => client.SearchAsync<Project>(It.IsAny<Func<SearchDescriptor<Project>, ISearchRequest>>(), default))
                .ReturnsAsync(searchResponse.Object);

            _configurationMock.Setup(c => c["Elasticsearch:IndexName"]).Returns("projects");

            // Act
            var result = await _projectService.GetAllProjectsFromElasticSearchAsync();

            // Assert
            Assert.Equal(projects, result);
        }

        [Fact]
        public async Task GetProjectByIdFromElasticSearchAsync_ShouldReturnProject()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "Test Project" };

            var searchResponse = new Mock<ISearchResponse<Project>>();
            searchResponse.Setup(s => s.Documents).Returns(new List<Project> { project });
            searchResponse.Setup(s => s.IsValid).Returns(true);

            _elasticClientMock.Setup(client => client.SearchAsync<Project>(It.IsAny<Func<SearchDescriptor<Project>, ISearchRequest>>(), default))
                .ReturnsAsync(searchResponse.Object);

            // Act
            var result = await _projectService.GetProjectByIdFromElasticSearchAsync(1);

            // Assert
            Assert.Equal(project, result);
        }
    }
}