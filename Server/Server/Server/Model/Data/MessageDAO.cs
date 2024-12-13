using Server.Model.DTO;
using System.Data;
using System.Globalization;

namespace Server.Model.Data
{
    /// <summary>
    /// Classe permettant d'accéder aux données des messages.
    /// </summary>
    public class MessageDAO : IMessageDAO
    {
        private IDatabase database;


        public MessageDAO(IDatabase database)
        {
            this.database = database;
        }

        /// <inheritdoc />
        public List<MessageDTO> GetConversation(string username1, string username2)
        {
            List<MessageDTO> messages = new List<MessageDTO>();
            database.Connect();

            try
            {
                string query = @"
                SELECT m.content, m.Timestamp, u1.username AS senderUsername, u2.username AS receiverUsername
                FROM messages m
                INNER JOIN user u1 ON m.sender_id = u1.idUser
                INNER JOIN user u2 ON m.receiver_id = u2.idUser
                WHERE 
                    (u1.username = @username1 AND u2.username = @username2)
                    OR 
                    (u1.username = @username2 AND u2.username = @username1)
                ORDER BY m.Timestamp ASC";

                var parameters = new Dictionary<string, object>
                {
                    { "@username1", username1 },
                    { "@username2", username2 }
                };

                var result = database.ExecuteQuery(query, parameters);

                foreach (DataRow row in result.Rows)
                {
                    MessageDTO message = new MessageDTO(
                        row["senderUsername"].ToString(),
                        row["receiverUsername"].ToString(),
                        row["content"].ToString(),
                        Convert.ToDateTime(row["Timestamp"], CultureInfo.InvariantCulture)
                    );

                    messages.Add(message);
                }
            }
            finally
            {
                database.Disconnect();
            }

            return messages;
        }
    }
}
