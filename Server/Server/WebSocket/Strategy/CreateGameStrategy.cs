using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model;
using System.Globalization;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie pour la création d'une nouvelle partie de jeu
    /// </summary>
    public class CreateGameStrategy : IStrategy
    {
        private IGameDAO gameDAO;
        public CreateGameStrategy()
        {
            this.gameDAO = new GameDAO();
        }

        /// <summary>
        /// Exécute la création d'une nouvelle partie
        /// </summary>
        /// <param name="player">Le joueur qui créé la partie</param>
        /// <param name="data">Les données du message sous forme de tableau de chaînes</param>
        /// <param name="gameType">Le type de partie concernée ("custom" ou "matchmaking")</param>
        /// <param name="response">La réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Le type de réponse (modifié par référence)</param>
        public void Execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            int size = Convert.ToInt16(data[3]);
            string rule = data[4];
            string name = data[7];
            float komi = float.Parse(data[6], CultureInfo.InvariantCulture.NumberFormat);
            int handicap = int.Parse(data[8]); 
            if (gameType == "custom") // la partie est personnalisée
            {
                int id = Server.CustomGames.Count + 1; // Génération de l'id de la partie
                Game newGame = new Game(size, rule, komi, name ,handicap);
                newGame.AddPlayer(player);
                Server.CustomGames[id] = newGame;
                gameDAO.InsertGame(newGame); // Ajout de la partie dans le dictionnaire des parties
                player.User.Token = data[2];
                Server.CustomGames[id].Player1 = player; // Ajout du client en tant que joueur 1
                response = $"{id}-"; // Renvoi de l'id de la partie créée
                type = "Send_";
            }
            else if (gameType == "matchmaking")
            {
                int id = Server.MatchmakingGames.Count + 1; // Génération de l'id de la partie
                Game newGame = new Game(size, rule, 0, " ",0);
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
