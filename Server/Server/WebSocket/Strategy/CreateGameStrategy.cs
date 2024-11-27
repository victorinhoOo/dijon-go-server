using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Le joueur créé une partie 
    /// </summary>
    public class CreateGameStrategy : IStrategy
    {
        private IGameDAO gameDAO;
        public CreateGameStrategy()
        {
            this.gameDAO = new GameDAO();
        }
        public void execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            int size = Convert.ToInt16(data[3]);
            string rule = data[4];
            if (gameType == "custom") // la partie est personnalisée
            {
                int id = Server.Games.Count + 1; // Génération de l'id de la partie
                Game newGame = new Game(size, rule);
                newGame.AddPlayer(player);
                Server.Games[id] = newGame;
                gameDAO.InsertGame(newGame); // Ajout de la partie dans le dictionnaire des parties
                player.User.Token = data[2];
                Server.Games[id].Player1 = player; // Ajout du client en tant que joueur 1
                response = $"{id}-"; // Renvoi de l'id de la partie créée
                type = "Send_";
            }
            else if (gameType == "matchmaking")
            {
                int id = Server.MatchmakingGames.Count + 1; // Génération de l'id de la partie
                Game newGame = new Game(size, rule);
                newGame.AddPlayer(player);
                Server.MatchmakingGames[id] = newGame;
                player.User.Token = data[2];
                Server.MatchmakingGames[id].Player1 = player;
                response = $"{id}-"; // Renvoi del'id de la partie créée
                type = "Send_";
            }
        }
    }
}
