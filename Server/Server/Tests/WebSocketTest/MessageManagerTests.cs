using Xunit;
using WebSocket.Model.Managers;
using Tests.WebSockets.FakeDAO;
using WebSocket.Model.DTO;
using WebSocket.Model.DAO;
using System;

namespace Tests.WebSocketTest
{
    public class MessageManagerTests
    {
        private readonly FakeUserDAO fakeUserDAO;
        private readonly FakeMessageDAO fakeMessageDAO;
        private readonly MessageManager messageManager;

        public MessageManagerTests()
        {
            // Setup fake DAOs
            fakeUserDAO = new FakeUserDAO();
            fakeMessageDAO = new FakeMessageDAO();

            // Create MessageManager with fake dependencies
            messageManager = new MessageManager(fakeMessageDAO, fakeUserDAO);
        }

        [Fact]
        public void AddMessage_ValidUsers_MessageAdded()
        {
            // Arrange
            string senderUsername = "Alice";
            string receiverUsername = "Bob";
            string content = "Hello Bob!";

            // Act
            messageManager.AddMessage(senderUsername, receiverUsername, content);

            // Assert
            var messages = fakeMessageDAO.GetAllMessages();
            Assert.Single(messages);
            Assert.Equal(content, messages[0].Content);
            Assert.Equal(1, messages[0].SenderId); // Alice
            Assert.Equal(2, messages[0].ReceiverId); // Bob
        }
    }

    public class FakeMessageDAO : IMessageDAO
    {
        private readonly List<MessageDTO> messages = new List<MessageDTO>();

        public void InsertMessage(MessageDTO message)
        {
            messages.Add(message);
        }

        public List<MessageDTO> GetAllMessages()
        {
            return messages;
        }
    }
}