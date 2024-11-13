using Server.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;

namespace WebSocket.Model.DAO
{
    public class UserDAO : IUserDAO
    {

        private IDatabase database;

        public UserDAO()
        {
            this.database = new SQLiteDatabase("Data Source= ../../../../Server/dgs.db");

        }
        public GameUserDTO GetUserByToken(string token)
        {
            GameUserDTO userResult = null;
            database.Connect();

            try
            {
                // on récupére l'utilisateur associé au token
                string query = @"
                SELECT u.idUser, u.username, u.elo
                FROM user u
                INNER JOIN tokenuser t ON u.idToken = t.idToken
                WHERE t.token = @token";

                var parameters = new Dictionary<string, object>
                {
                    { "@token", token }
                };

                var result = database.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 0)
                {
                    userResult = new GameUserDTO
                    {
                        Name = result.Rows[0]["username"].ToString(),
                        Elo = Convert.ToInt32(result.Rows[0]["elo"])
                    };
                }
            }
            finally
            {
                database.Disconnect();
            }
            return userResult;
        }
    }
}
