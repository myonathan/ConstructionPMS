using System.Threading.Tasks;
using ConstructionPMS.Services;
using ConstructionPMS.Shared.Exceptions;
using Moq;
using Xunit;

namespace ConstructionPMS.Services.Tests
{
    public class NotificationServiceTests
    {
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _notificationService = new NotificationService();
        }

        [Fact]
        public async Task SendNotificationAsync_ValidMessage_ShouldNotThrow()
        {
            // Arrange
            string message = "Test notification";

            // Act
            await _notificationService.SendNotificationAsync(message);

            // Assert
            // No exception should be thrown
        }

        [Fact]
        public async Task SendNotificationAsync_EmptyMessage_ShouldThrowCustomException()
        {
            // Arrange
            string message = "";

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _notificationService.SendNotificationAsync(message));
        }
    }
}