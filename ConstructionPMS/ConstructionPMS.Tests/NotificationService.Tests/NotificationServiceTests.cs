using System;
using System.Threading.Tasks;
using ConstructionPMS.Infrastructure.Kafka;
using Moq;
using Xunit;

namespace ConstructionPMS.Services.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<IKafkaProducerService> _kafkaProducerMock;
        private readonly NotificationService.NotificationService _notificationService;

        public NotificationServiceTests()
        {
            // Mock the IKafkaProducerService
            _kafkaProducerMock = new Mock<IKafkaProducerService>();
            _notificationService = new NotificationService.NotificationService(_kafkaProducerMock.Object);
        }

        [Fact]
        public async Task SendNotificationAsync_ValidMessage_ShouldSendMessage()
        {
            // Arrange
            var message = "Test notification message";

            // Set up the ProduceAsync method to do nothing (or you can verify it was called)
            _kafkaProducerMock.Setup(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _notificationService.SendNotificationAsync(message);

            // Assert
            _kafkaProducerMock.Verify(k => k.ProduceAsync("notifications", message), Times.Once);
        }

        [Fact]
        public async Task SendNotificationAsync_NullMessage_ShouldThrowArgumentException()
        {
            // Arrange
            string message = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _notificationService.SendNotificationAsync(message));
            Assert.Equal("Message cannot be null or empty. (Parameter 'message')", exception.Message);
        }

        [Fact]
        public async Task SendNotificationAsync_EmptyMessage_ShouldThrowArgumentException()
        {
            // Arrange
            string message = string.Empty;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _notificationService.SendNotificationAsync(message));
            Assert.Equal("Message cannot be null or empty. (Parameter 'message')", exception.Message);
        }
    }
}