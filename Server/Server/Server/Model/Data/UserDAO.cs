using System.Data;
using System.Text;

namespace Server.Model.Data
{
    /// <summary>
    /// Représente l'accès aux données des utilisateurs.
    /// </summary>
    public class UserDAO : IUserDAO
    {
        private readonly IDatabase database;
        private readonly ILogger<UserDAO> logger;

        public UserDAO(IDatabase database, ILogger<UserDAO> logger)
        {
            this.database = database;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public bool VerifyExists(User user)
        {
            bool res = false;
            database.Connect();

            string query = "SELECT COUNT(*) FROM user WHERE username = @username AND hashPwd = @hashPwd";
            var parameters = new Dictionary<string, object>
                {
                    {"@username", user.Username},
                    {"@hashPwd", user.Password}
                };

            var result = database.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0)
            {
                res = true;
            }

            database.Disconnect();
            this.logger.LogInformation($"L'utilisateur {user.Username} existe: {res}");
            return res;
        }

        /// <inheritdoc/>
        public bool Register(User user)
        {
            bool res = false;
            database.Connect();

            // Adapter la requête SQL en fonction de la présence du mot de passe
            string query = "INSERT INTO user (username, hashPwd, email, elo) VALUES (@username, @hashPwd, @email, @elo)";
            var parameters = new Dictionary<string, object>
                {
                    {"@username", user.Username},
                    {"@hashPwd", user.Password},
                    {"@email", user.Email},
                    {"@elo", 100} // Chaque nouveau joueur commence à 100 elo
                };

            if (!string.IsNullOrEmpty(user.Password)) // Gestion du stockage des comptes googles (sans mot de passe)
            {
                query = "INSERT INTO user (username, hashPwd, email, elo) VALUES (@username, @hashPwd, @email, @elo)";
            }
            else
            {
                query = "INSERT INTO user (username, email, elo) VALUES (@username, @email, @elo)";
            }

            // Exécution de la requête
            database.ExecuteNonQuery(query, parameters);
            res = true;

            database.Disconnect();
            logger.LogInformation($"L'utilisateur {user.Username} a été enregistré");
            return res;
        }


        /// <inheritdoc/>
        public bool Update(User user)
        {
            bool res = false;
            database.Connect();

            // Construction de la requête de mise à jour dynamique
            var queryBuilder = new StringBuilder("UPDATE user SET ");
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(user.Username))
            {
                queryBuilder.Append("username = @username, ");
                parameters.Add("@username", user.Username);
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                queryBuilder.Append("email = @email, ");
                parameters.Add("@email", user.Email);
            }
            if (!string.IsNullOrEmpty(user.Password))
            {
                queryBuilder.Append("hashPwd = @hashPwd, ");
                parameters.Add("@hashPwd", user.Password);
            }

            // ajoute la condition WHERE pour mettre à jour l'utilisateur
            queryBuilder.Length -= 2;
            queryBuilder.Append(" WHERE idUser = @idUser");
            parameters.Add("@idUser", user.Id);

            string query = queryBuilder.ToString();
            database.ExecuteNonQuery(query, parameters);
            res = true;

            database.Disconnect();
            logger.LogInformation($"L'utilisateur {user.Username} a été mis à jour");
            return res;
        }

        /// <inheritdoc/>
        public User GetUserByUsername(string username)
        {
            User user = null;
            database.Connect();

            string query = "SELECT * FROM user WHERE username = @username";
            var parameters = new Dictionary<string, object>
                {
                    {"@username", username}
                };

            var result = database.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                user = new User
                {
                    Username = result.Rows[0]["username"].ToString(),
                    Password = result.Rows[0]["hashPwd"].ToString(),
                    Email = result.Rows[0]["email"].ToString(),
                    
                };
            }

            database.Disconnect();
            logger.LogInformation($"Récupération de l'utilisateur {username}");
            return user;
        }

        /// <inheritdoc/>
        public Dictionary<string, int> GetTop5Users()
        {
            Dictionary<string, int> topUsers = new Dictionary<string, int>();
            database.Connect();

            // Requête SQL pour obtenir le top 5 des utilisateurs selon leur score Elo
            string query = "SELECT username, elo FROM user ORDER BY elo DESC LIMIT 5";
            var result = database.ExecuteQuery(query, new Dictionary<string, object>());

            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    string username = row["username"].ToString();
                    int elo = Convert.ToInt32(row["elo"]);

                    // ajout de l'utilisateur au leaderboard
                    topUsers[username] = elo;
                }
            }

            database.Disconnect();
            logger.LogInformation("Récupération des 5 meilleurs utilisateurs (noms et Elo) dans un dictionnaire.");
            return topUsers;
        }


    }
}
