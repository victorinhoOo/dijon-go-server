using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Server.Model.Managers;
using Server.Model.Data;
using Server.Model.DTO;

namespace Tests.Messages
{
    public class MessageManagerTests
    {
        private MessageManager messageManager;
        private FakeMessageDAO fakeMessageDAO;
        private FakeTokenDAO fakeTokenDAO;
        private Mock<ILogger<MessageManager>> mockLogger;

        public MessageManagerTests()
        {
            fakeMessageDAO = new FakeMessageDAO();
            fakeTokenDAO = new FakeTokenDAO(); // Utilisation du FakeTokenDAO
            mockLogger = new Mock<ILogger<MessageManager>>();
            messageManager = new MessageManager(fakeTokenDAO, fakeMessageDAO, mockLogger.Object);
        }

        [Fact]
        public void GetConversation_WithValidToken_ReturnsMessages()
        {
            // Arrange
            string token = "abc123"; // Token valide pour "victor"
            string recipient = "clem";

            // Act
            var result = messageManager.GetConversation(token, recipient);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Hello", result[0].Content);
            Assert.Equal("Hi", result[1].Content);
        }

        [Fact]
        public void GetConversation_WithNonExistingRecipient_ReturnsEmptyList()
        {
            // Arrange
            string token = "abc123"; // Token valide pour "victor"
            string recipient = "nonexistinguser";

            // Act
            var result = messageManager.GetConversation(token, recipient);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetConversation_LogsInformation()
        {
            // Arrange
            string token = "abc123"; // Token valide pour "victor"
            string recipient = "user2";

            // Act
            messageManager.GetConversation(token, recipient);

            // Assert
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(2));
        }
    }
}