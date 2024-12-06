using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model;
using System.Globalization;
using WebSocket.Strategy.Enumerations;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie pour la création d'une nouvelle partie de jeu
    /// </summary>
    public class CreateGameStrategy : IStrategy
    {
        private IGameDAO gameDAO;
        private GameManager gameManager;
        public CreateGameStrategy()
        {
            this.gameDAO = new GameDAO();
            this.gameManager = new GameManager();
        }

        /// <summary>
        /// Exécute la création d'une nouvelle partie
        /// </summary>
        /// <param name="player">Le joueur qui créé la partie</param>
        /// <param name="data">Les données du message sous forme de tableau de chaînes</param>
        /// <param name="gameType">Le type de partie concernée ("custom" ou "matchmaking")</param>
        /// <param name="response">La réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Le type de réponse (modifié par référence)</param>
        public void Execute(Client player, string[] data, GameType gameType, ref string response, ref string type)
        {
            if (gameType == GameType.CUSTOM) // la partie est personnalisée
            {
                int size = Convert.ToInt16(data[3]);
                string rule = data[4];
                string name = data[7];
                float komi = float.Parse(data[6], CultureInfo.InvariantCulture.NumberFormat);
                int handicap = int.Parse(data[8]);
                string colorHandicap = data[9];
                int id = Server.CustomGames.Count + 1; 
                GameConfiguration config = new GameConfiguration(size, rule, komi, name, handicap, colorHandicap);
                Game newGame = GameFactory.CreateCustomGame(config);
                player.User.Token = data[2];
                newGame.AddPlayer(player);
                Server.CustomGames[id] = newGame;
                newGame.Player1.User = this.gameManager.GetUserByToken(newGame.Player1.User.Token); //  récupération de l'utilisateur pour l'insertion en bdd
                gameDAO.InsertAvailableGame(newGame); // Ajout de la partie dans le dictionnaire des parties
                Server.CustomGames[id].Player1 = player; // Ajout du client en tant que joueur 1
                response = $"{id}-"; // Renvoi de l'id de la partie créée
                type = "Send_";
            }
            else if (gameType == GameType.MATCHMAKING)
            {
                int id = Server.MatchmakingGames.Count + 1;
                Game newGame = GameFactory.CreateMatchmakingGame();
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
