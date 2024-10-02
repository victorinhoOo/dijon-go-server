﻿namespace Server.Model.Data
{
    /// <summary>
    /// Classe pour gérer les opérations liées aux tokens.
    /// </summary>
    public class TokenDAO: ITokenDAO
    {
        private readonly IDatabase database;

        public TokenDAO(IDatabase database)
        {
            this.database = database;
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
                    { "@date", DateTime.Now }
                };

                database.ExecuteNonQuery(query, parameters);

                // Récupère l'idToken généré après l'insertion
                string selectTokenIdQuery = "SELECT LAST_INSERT_ID() as idToken";
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

                database.CommitTransaction();
            }
            catch (Exception ex)
            {
                // Annule la transaction en cas d'erreur
                database.RollbackTransaction();
                throw new Exception("An error occurred during token insertion and user update.", ex);
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
                SELECT u.idUser, u.username, u.email, u.hashPwd
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
                    string password = result.Rows[0]["hashPwd"].ToString();
                    string email = result.Rows[0]["email"].ToString();                 

                    user = new User(idUser, username, password, email);
                }
            }
            finally
            {
                database.Disconnect();
            }

            return user;
        }
    }
}
