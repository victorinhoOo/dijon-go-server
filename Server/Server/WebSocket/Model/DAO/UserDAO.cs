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
                        Elo = Convert.ToInt32(result.Rows[0]["elo"]),
                        Token = token
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
            string query = "UPDATE user SET elo = @newElo WHERE idToken = (SELECT idToken FROM tokenuser WHERE token = @token)";

            var parameters = new Dictionary<string, object>
            {
                { "@newElo", newElo },
                { "@token", token }
            };

            try
            {
                // Assurez-vous que la méthode ExecuteNonQuery accepte bien un dictionnaire avec des paramètres
                database.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating elo: " + ex.Message);
            }
            finally
            {
                database.Disconnect();
            }
        }


    }
}
