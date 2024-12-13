using DotNetEnv;
using Server.Model.Data;
using WebSocket.Model.DTO;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Classe permettant d'accéder aux données des utilisateurs.
    /// </summary>
    public class UserDAO : IUserDAO
    {

        private IDatabase database;

        public UserDAO()
        {
            string sqliteConnectionString = Env.GetString("SQLITE_CONNECTION_STRING");
            this.database = new SQLiteDatabase(sqliteConnectionString);
        }
        /// <inheritdoc/>
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
                        Id = Convert.ToInt32(result.Rows[0]["idUser"]),
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public int GetIdByUsername(string username)
        {
            database.Connect();
            int id = -1;
            try
            {
                string query = "SELECT idUser FROM user WHERE username = @username";
                var parameters = new Dictionary<string, object>
                {
                    { "@username", username }
                };
                var result = database.ExecuteQuery(query, parameters);
                if (result.Rows.Count > 0)
                {
                    id = Convert.ToInt32(result.Rows[0]["idUser"]);
                }
            }
            finally
            {
                database.Disconnect();
            }
            return id;
        }
    }
}
