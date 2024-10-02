using System.Text;

namespace Server.Model.Data
{
    /// <summary>
    /// Représente l'accès aux données des utilisateurs.
    /// </summary>
    public class UserDAO : IUserDAO
    {
        private readonly IDatabase database;

        public UserDAO(IDatabase database)
        {
            this.database = database;
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
            return res;
        }

        /// <inheritdoc/>
        public bool Register(User user)
        {
            bool res = false;
            database.Connect();

            string query = "INSERT INTO user (username, hashPwd, email) VALUES (@username, @hashPwd, @email)";
            var parameters = new Dictionary<string, object>
                {
                    {"@username", user.Username},
                    {"@hashPwd", user.Password},
                    {"@email", user.Email}
                };

            database.ExecuteNonQuery(query, parameters);
            res = true;

            database.Disconnect();
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
                    Email = result.Rows[0]["email"].ToString()
                };
            }

            database.Disconnect();
            return user;
        }
    }
}
