namespace Server.Model.DTO
{
    /// <summary>
    /// Représente un message entre deux joueurs.
    /// </summary>
    public class MessageDTO
    {
        private string senderUsername;
        private string receiverUsername;
        private string content;
        private DateTime timestamp;

        /// <summary>
        /// Renvoi l'id de l'envoyeur
        /// </summary>
        public string SenderUsername { get => senderUsername; }
        /// <summary>
        /// Renvoi l'id du receveur
        /// </summary>
        public string ReceiverUsername { get => receiverUsername; }

        /// <summary>
        /// Renvoi le contenu du message
        /// </summary>
        public string Content { get => content; }

        /// <summary>
        /// Renvoi le timestamp du message
        /// </summary>
        public DateTime Timestamp { get => timestamp; }


        public MessageDTO(string senderUsername, string receiverUsername, string content, DateTime timestamp)
        {
            this.senderUsername = senderUsername;
            this.receiverUsername = receiverUsername;
            this.content = content;
            this.timestamp = timestamp;
        }
    }
}
