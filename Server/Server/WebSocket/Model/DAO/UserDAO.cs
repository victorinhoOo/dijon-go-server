using Server.Model.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        public void UpdateEloByToken(string token, int newElo)
        {
            database.Connect();

            try
            {
                // Requête pour mettre à jour l'Elo de l'utilisateur en fonction du token
                string query = @"UPDATE user SET elo = @newElo WHERE idUser = (
                SELECT u.idUser
                FROM user u
                INNER JOIN tokenuser t ON u.idToken = t.idToken
                WHERE t.token = @token)";

                // Paramètres pour la requête
                var parameters = new Dictionary<string, object>
                {
                    { "@newElo", newElo },
                    { "@token", token }
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
