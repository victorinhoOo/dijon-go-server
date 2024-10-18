using Server.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DAO
{
    public class UserDAO : IUserDAO
    {

        private IDatabase database;

        public UserDAO()
        {
            this.database = new SQLiteDatabase("Data Source= ../../../../Server/dgs.db");

        }
        public string GetUsernameByToken(string token)
        {
            string usernameResult = null;
            database.Connect();

            try
            {
                // on récupére l'utilisateur associé au token
                string query = @"
                SELECT u.idUser, u.username
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
                    usernameResult = result.Rows[0]["username"].ToString();
                }
            }
            finally
            {
                database.Disconnect();
            }
            return usernameResult;
        }
    }
}
