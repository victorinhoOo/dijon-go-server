using Moq;
using System.Net.Sockets;
using System.Text;
using Tests.WebSocket.WebSocket.Tests;
using Xunit;

namespace WebSocket.Tests
{
    public class ServerTest
    {
        [Fact]
        public void Test_MessageIsHandshakeRequest()
        {
            // Arrange
            Server server = new Server();
            string handshakeMessage = "GET /chat HTTP/1.1\r\nHost: server.example.com\r\nUpgrade: websocket\r\n";
            string normalMessage = "Hello world";

            // Act & Assert
            Assert.True(server.MessageIsHandshakeRequest(handshakeMessage));
            Assert.False(server.MessageIsHandshakeRequest(normalMessage));
        }

        [Fact]
        public void Test_TreatMessage_CreateCustomGame()
        {
            // Arrange
            Server server = new Server();
            Client client = new MockClient();
            string message = "0-Create-token123-9-standard-custom";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            string response = "";

            // Act
            server.TreatMessage(messageBytes, client, ref message, ref response);

            // Assert
            Assert.Matches(@"^1-$", response); // Vérifie que la réponse est de la forme "1-" (ID de la partie créée)
            Assert.Single(Server.CustomGames); // Vérifie qu'une partie a été créée
            Assert.Equal(client, Server.CustomGames[1].Player1); // Vérifie que le client est bien le joueur 1
        }

        [Fact]
        public void Test_TreatMessage_JoinCustomGame()
        {
            // Arrange
            Server server = new Server();
            Client host = new MockClient();
            Client joiner = new MockClient();
            
            // Créer d'abord une partie
            string createMessage = "0-Create-token123-9-standard-custom";
            byte[] createMessageBytes = Encoding.UTF8.GetBytes(createMessage);
            string createResponse = "";
            server.TreatMessage(createMessageBytes, host, ref createMessage, ref createResponse);

            // Rejoindre la partie
            string joinMessage = "0-Join-token456-1-custom";
            byte[] joinMessageBytes = Encoding.UTF8.GetBytes(joinMessage);
            string joinResponse = "";

            // Act
            server.TreatMessage(joinMessageBytes, joiner, ref joinMessage, ref joinResponse);

            // Assert
            Assert.Equal("1-", joinResponse);
            Assert.True(Server.CustomGames[1].IsFull);
            Assert.Equal(joiner, Server.CustomGames[1].Player2);
        }

        [Fact]
        public void Test_TreatMessage_PlaceStone()
        {
            // Arrange
            Server server = new Server();
            Client player1 = new MockClient();
            Client player2 = new MockClient();
            
            // Créer et démarrer une partie
            SetupFullGame(server, player1, player2);

            // Placer une pierre
            string placeMessage = "1-Stone-2-3";  // Format: gameId-Stone-x-y
            byte[] placeMessageBytes = Encoding.UTF8.GetBytes(placeMessage);
            string placeResponse = "";

            // Act
            server.TreatMessage(placeMessageBytes, player1, ref placeMessage, ref placeResponse);

            // Assert
            Assert.Matches(@"^1-Stone-2-3$", placeResponse);
        }

        private void SetupFullGame(Server server, Client player1, Client player2)
        {
            string createMessage = "0-Create-token123-9-standard-custom";
            byte[] createMessageBytes = Encoding.UTF8.GetBytes(createMessage);
            string createResponse = "";
            server.TreatMessage(createMessageBytes, player1, ref createMessage, ref createResponse);

            string joinMessage = "0-Join-token456-1-custom";
            byte[] joinMessageBytes = Encoding.UTF8.GetBytes(joinMessage);
            string joinResponse = "";
            server.TreatMessage(joinMessageBytes, player2, ref joinMessage, ref joinResponse);
        }
    }
} 