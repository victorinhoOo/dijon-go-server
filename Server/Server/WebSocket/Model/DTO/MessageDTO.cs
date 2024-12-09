using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DTO
{
    /// <summary>
    /// Représente un message entre deux joueurs.
    /// </summary>
    public class MessageDTO
    {
        private int senderId;
        private int receiverId;
        private string content;
        private DateTime timestamp;

        /// <summary>
        /// Renvoi l'id de l'envoyeur
        /// </summary>
        public int SenderId { get => senderId; }
        /// <summary>
        /// Renvoi l'id du receveur
        /// </summary>
        public int ReceiverId { get => receiverId; }

        /// <summary>
        /// Renvoi le contenu du message
        /// </summary>
        public string Content { get => content; }

        /// <summary>
        /// Renvoi le timestamp du message
        /// </summary>
        public DateTime Timestamp { get => timestamp; }


        public MessageDTO(int senderId, int receiverId, string content, DateTime timestamp)
        {
            this.senderId = senderId;
            this.receiverId = receiverId;
            this.content = content;
            this.timestamp = timestamp;
        }
    }
}
