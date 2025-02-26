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
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ValidTask_ShouldReturnCreatedTask()
        {
            // Arrange
            var task = new ProjectTask("Test Task", Domain.Entities.TaskStatus.Pending, DateTime.Now.AddDays(7), Guid.NewGuid());
            _taskRepositoryMock.Setup(repo => repo.AddAsync(task)).ReturnsAsync(task);

            // Act
            var result = await _taskService.CreateTaskAsync(task);

            // Assert
            Assert.Equal(task, result);
        }

        [Fact]
        public async Task CreateTaskAsync_NullTask_ShouldThrowCustomException()
        {
            // Arrange
            ProjectTask task = null;

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _taskService.CreateTaskAsync(task));
        }

        [Fact]
        public async Task GetTaskByIdAsync_ExistingId_ShouldReturnTask()
        {
            // Arrange
            var task = new ProjectTask("Test Task", Domain.Entities.TaskStatus.Pending, DateTime.Now.AddDays(7), Guid.NewGuid());
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(task.Id)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(task.Id);

            // Assert
            Assert.Equal(task, result);
        }

        [Fact]
        public async Task GetTaskByIdAsync_NonExistingId_ShouldThrowCustomException()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((ProjectTask)null);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _taskService.GetTaskByIdAsync(taskId));
        }
    }
}