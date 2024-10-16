using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DAO
{
    public class GameDAO
    {
        private IDatabase database;

        public GameDAO()
        {
            this.database = new MySqlDatabase("Server=localhost;Database=dgs;User=root;Password=''");
            
        }

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
    }
}
