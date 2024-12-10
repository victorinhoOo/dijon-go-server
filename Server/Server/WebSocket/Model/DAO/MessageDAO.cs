using DotNetEnv;
using Server.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Classe permettant d'accéder aux données des messages.
    /// </summary>
    public class MessageDAO : IMessageDAO
    {

        private IDatabase database;
        public MessageDAO()
        {
            string sqliteConnectionString = Env.GetString("SQLITE_CONNECTION_STRING");
            this.database = new SQLiteDatabase(sqliteConnectionString);
        }
        /// <inheritdoc />
        public void InsertMessage(MessageDTO message)
        {
            database.Connect();

            try
            {
                string query = @"
                INSERT INTO messages (sender_id, receiver_id, content, Timestamp)
                VALUES (@senderId, @receiverId, @content, @timestamp)";

                var parameters = new Dictionary<string, object>
                {
                    { "@senderId", message.SenderId },
                    { "@receiverId", message.ReceiverId },
                    { "@content", message.Content },
                    { "@timestamp", message.Timestamp }
                };

                database.ExecuteNonQuery(query, parameters);
            }
            finally
            {
                database.Disconnect();
            }
        }
    }
}
