using Server.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Gère les opérations CRUD liées aux parties disponibles en bdd
    /// </summary>
    public class GameDAO: IGameDAO
    {
        private IDatabase database;

        public GameDAO()
        {
            this.database = new SQLiteDatabase("Data Source=./Server/dgs.db");

        }

        ///<inheritdoc/>
        public bool InsertGame(Game game)
        {
            this.database.Connect();
            bool res = false;
            string query = "insert into availablegame (id,title,size) values (@id,@title,@size);";
            var parameters = new Dictionary<string, object>
                {
                    {"@id", game.Id},
                    {"@title", $"Partie numéro {game.Id}"},
                    {"@size", game.Size}
                };
            database.ExecuteNonQuery(query, parameters);
            res = true;
            this.database.Disconnect();
            return res;
        }

        ///<inheritdoc/>
        public void DeleteGame(int id)
        {
            this.database.Connect();
            string query = "delete from availablegame where id = @id;";
            var parameters = new Dictionary<string, object>
                {
                    {"@id", id}
                };
            database.ExecuteNonQuery(query, parameters);
            this.database.Disconnect();
        }
    }
}
