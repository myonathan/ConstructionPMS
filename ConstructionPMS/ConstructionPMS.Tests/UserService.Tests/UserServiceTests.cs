using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Services;
using ConstructionPMS.Shared.Exceptions;
using Moq;
using Nest;
using Xunit;

namespace ConstructionPMS.Services.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository>_userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var user = new User("TestUser ", "testuser@example.com", "Admin", "");
            _userRepositoryMock.Setup(repo => repo.AddAsync(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.CreateUserAsync(user);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task CreateUserAsync_NullUser_ShouldThrowCustomException()
        {
            // Arrange
            User user = null;

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _userService.CreateUserAsync(user));
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingId_ShouldReturnUser()
        {
            // Arrange
            var user = new User("TestUser ", "testuser@example.com", "Admin", "");

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(user.Id);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingId_ShouldThrowCustomException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _userService.GetUserByIdAsync(userId));
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User("User 1", "user1@example.com", "Admin", ""),
                new User("User 2", "user2@example.com", "User", "")
            };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(users.Count, result.Count());
        }
    }
}