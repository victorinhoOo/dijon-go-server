namespace Server.Model.Data
{
    /// <summary>
    /// Classe pour gérer les opérations liées aux tokens.
    /// </summary>
    public class TokenDAO: ITokenDAO
    {
        private readonly IDatabase database;
        private ILogger<TokenDAO> logger;

        public TokenDAO(IDatabase database, ILogger<TokenDAO> logger)
        {
            this.database = database;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public bool CheckToken(string username, string token)
        {
            bool res = false;
            database.Connect();

            // Requête SQL pour vérifier si le token est associé à l'utilisateur 
            string query = @"
            SELECT COUNT(*) 
            FROM user u 
            INNER JOIN tokenuser t ON u.idToken = t.idToken
            WHERE u.username = @username AND t.token = @token";

            var parameters = new Dictionary<string, object>
            {
                { "@username", username },
                { "@token", token }
            };

            var result = database.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0)
            {
                res = true;
            }

            database.Disconnect();
            this.logger.LogInformation($"Le token {token} est associé à l'utilisateur {username}: {res}");
            return res;
        }

        /// <inheritdoc/>
        public bool InsertTokenUser(string username, string token)
        {
            bool result = false;
            database.Connect();

            try
            {
                database.BeginTransaction();

                // Insère le token dans la table `tokenuser`
                string query = @"INSERT INTO tokenuser (token, date) VALUES (@token, @date)";
                var parameters = new Dictionary<string, object>
                {
                    { "@token", token },
                    { "@date", DateTime.Now.ToString() }
                };

                database.ExecuteNonQuery(query, parameters);

                // Récupère l'idToken généré après l'insertion
                string selectTokenIdQuery = "SELECT last_insert_rowid() as idToken";
                var tokenResult = database.ExecuteQuery(selectTokenIdQuery, null);

                if (tokenResult.Rows.Count > 0)
                {
                    int idToken = Convert.ToInt32(tokenResult.Rows[0]["idToken"]);

                    // Met à jour l'utilisateur avec cet idToken dans la table `user`
                    string updateUserQuery = "UPDATE user SET idToken = @idToken WHERE username = @username";
                    var updateParameters = new Dictionary<string, object>
                    {
                        { "@idToken", idToken },
                        { "@username", username }
                    };

                    database.ExecuteNonQuery(updateUserQuery, updateParameters);
                    result = true;
                }
                logger.LogInformation($"Le token {token} a été inséré pour l'utilisateur {username}");
                database.CommitTransaction();
            }
            catch (System.Exception ex)
            {
                // Annule la transaction en cas d'erreur
                database.RollbackTransaction();
                logger.LogError("Une erreur a eu lieu pendant l'insertion du token:", ex);
                throw new Exception("Une erreur a eu lieu pendant l'insertion du token:", ex);
            }
            finally
            {
                database.Disconnect();
            }

            return result;
        }

        /// <inheritdoc/>
        public User GetUserByToken(string token)
        {
            User user = null;
            database.Connect();

            try
            {
                // on récupére l'utilisateur associé au token
                string query = @"
                SELECT u.idUser, u.username, u.email, u.elo
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
                    int idUser = Convert.ToInt32(result.Rows[0]["idUser"]);
                    string username = result.Rows[0]["username"].ToString();
                    string email = result.Rows[0]["email"].ToString();
                    int elo = Convert.ToInt32(result.Rows[0]["elo"]);

                    user = new User
                    {
                        Id = idUser,
                        Username = username,
                        Email = email,
                        Elo = elo,
                    };
                }
            }
            finally
            {
                database.Disconnect();
            }
            logger.LogInformation($"L'utilisateur {user.Username} est associé au token {token}");
            return user;
        }
    }
}
