using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;
using WebSocket;

namespace Tests.WebSocketTest
{
    public class MockClient : IClient
    {
        private Queue<byte[]> messagesToReceive = new Queue<byte[]>();
        private List<byte[]> sentMessages = new List<byte[]>();
        private GameUserDTO user = new GameUserDTO();

        public GameUserDTO User
        {
            get => user;
            set => user = value;
        }

        public void QueueMessageToReceive(byte[] message)
        {
            messagesToReceive.Enqueue(message);
        }

        public List<byte[]> GetSentMessages()
        {
            return sentMessages;
        }

        public byte[] ReceiveMessage()
        {
            if (messagesToReceive.Count == 0)
                throw new InvalidOperationException("No messages queued for receiving");
            return messagesToReceive.Dequeue();
        }

        public void SendMessage(byte[] bytes)
        {
            sentMessages.Add(bytes);
        }

        void IClient.ChangeUser(GameUserDTO user)
        {
            this.user.Token = user.Token;
            this.user.Name = user.Name;
            this.user.Elo = user.Elo;
            this.user.Id = user.Id;
        }
    }
}
