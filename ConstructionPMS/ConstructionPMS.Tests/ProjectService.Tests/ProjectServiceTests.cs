using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Services;
using Moq;
using Xunit;

namespace ConstructionPMS.Services.Tests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
        }

        [Fact]
        public async Task CreateProject_ShouldAddProject_WhenValid()
        {
            // Arrange
            var project = new Project
            {
                ProjectName = "New Project",
                ProjectLocation = "Location A",
                ProjectStage = ProjectStage.Concept,
                ProjectCategory = ProjectCategory.Education,
                ConstructionStartDate = DateTime.UtcNow.AddDays(30), // Future date
                ProjectDetails = "Project details here.",
                ProjectCreatorId = Guid.NewGuid()
            };

            // Act
            await _projectService.CreateProjectAsync(project);

            // Assert
            _projectRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task CreateProject_ShouldThrowValidationException_WhenConstructionStartDateIsInThePast()
        {
            // Arrange
            var project = new Project
            {
                ProjectName = "New Project",
                ProjectLocation = "Location A",
                ProjectStage = ProjectStage.Concept,
                ProjectCategory = ProjectCategory.Education,
                ConstructionStartDate = DateTime.UtcNow.AddDays(-1), // Past date
                ProjectDetails = "Project details here.",
                ProjectCreatorId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _projectService.CreateProjectAsync(project));
        }

        [Fact]
        public async Task GetAllProjects_ShouldReturnListOfProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { ProjectName = "Project 1" },
                new Project { ProjectName = "Project 2" }
            };

            _projectRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projects);

            // Act
            var result = await _projectService.GetAllProjectsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProjectById_ShouldReturnProject_WhenExists()
        {
            // Arrange
            var projectId = 123456; // Example project ID
            var project = new Project { ProjectName = "Project 1" };

            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(project.ProjectId)).ReturnsAsync(project);

            // Act
            var result = await _projectService.GetProjectByIdAsync(project.ProjectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Project 1", result.ProjectName);
        }

        [Fact]
        public async Task GetProjectById_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var projectId = 123456; // Example project ID
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act
            var result = await _projectService.GetProjectByIdAsync(projectId);

            // Assert
            Assert.Null(result);
        }

        // Additional tests for UpdateProject and DeleteProject can be added here
    }
}