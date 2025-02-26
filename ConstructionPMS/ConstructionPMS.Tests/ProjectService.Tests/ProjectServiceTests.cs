using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Services;
using ConstructionPMS.Shared.Exceptions;
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
            _projectService = new ProjectService(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ValidProject_ShouldReturnCreatedProject()
        {
            // Arrange
            var project = new Project("Test Project", "Test Project Description", DateTime.Now, DateTime.Now.AddMonths(1));
            _projectRepositoryMock.Setup(repo => repo.AddAsync(project)).ReturnsAsync(project);

            // Act
            var result = await _projectService.CreateProjectAsync(project);

            // Assert
            Assert.Equal(project, result);
        }

        [Fact]
        public async Task CreateProjectAsync_NullProject_ShouldThrowCustomException()
        {
            // Arrange
            Project project = null;

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _projectService.CreateProjectAsync(project));
        }

        [Fact]
        public async Task GetProjectByIdAsync_ExistingId_ShouldReturnProject()
        {
            // Arrange
            var project = new Project("Test Project", "Test Project Description", DateTime.Now, DateTime.Now.AddMonths(1));
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(project.Id)).ReturnsAsync(project);

            // Act
            var result = await _projectService.GetProjectByIdAsync(project.Id);

            // Assert
            Assert.Equal(project, result);
        }

        [Fact]
        public async Task GetProjectByIdAsync_NonExistingId_ShouldThrowCustomException()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _projectService.GetProjectByIdAsync(projectId));
        }
    }
}