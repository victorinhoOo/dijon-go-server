using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;
using WebSocket;


namespace Tests.WebSockets
{
    public class ServerTest
    {
        [Fact]
        public void Test_MessageIsHandshakeRequest()
        {
            // Arrange
            WebSocket.Server server = new WebSocket.Server();
            string handshakeMessage = "GET /chat HTTP/1.1\r\nHost: server.example.com\r\nUpgrade: websocket\r\n";
            string normalMessage = "Hello world";

            // Act & Assert
            Assert.True(server.MessageIsHandshakeRequest(handshakeMessage));
            Assert.False(server.MessageIsHandshakeRequest(normalMessage));
        }
    }
}
