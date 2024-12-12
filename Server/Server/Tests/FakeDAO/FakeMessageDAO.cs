using Server.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Model.Data
{
    /// <summary>
    /// FakeDAO pour les tests des opérations sur les messages
    /// </summary>
    public class FakeMessageDAO : IMessageDAO
    {
        private List<MessageDTO> messages;

        public FakeMessageDAO()
        {
            messages = new List<MessageDTO>
            {
                new MessageDTO("victor", "clem", "Hello", DateTime.Parse("2024-03-20 10:00:00")),
                new MessageDTO("clem", "victor", "Hi", DateTime.Parse("2024-03-20 10:01:00")),
                new MessageDTO("victor", "clem", "Hey", DateTime.Parse("2024-03-20 10:02:00")),
            };
        }

        public List<MessageDTO> GetConversation(string username1, string username2)
        {
            return messages.Where(m =>
                (m.SenderUsername == username1 && m.ReceiverUsername == username2) ||
                (m.SenderUsername == username2 && m.ReceiverUsername == username1))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }
    }
}