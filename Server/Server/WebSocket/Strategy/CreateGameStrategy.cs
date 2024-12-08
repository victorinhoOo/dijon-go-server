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
using WebSocket.Model.Managers;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie pour la création d'une nouvelle partie de jeu
    /// </summary>
    public class CreateGameStrategy : IStrategy
    {

        // Constantes pour les index de tableau
        private const int SIZE_INDEX = 3;
        private const int RULE_INDEX = 4;
        private const int KOMI_INDEX = 5;
        private const int NAME_INDEX = 6;
        private const int HANDICAP_INDEX = 7;
        private const int COLOR_HANDICAP_INDEX = 8;

        private AvailableGameManager availableGameManager;

        public CreateGameStrategy()
        {
            this.availableGameManager = new AvailableGameManager();
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
                int size = Convert.ToInt16(data[SIZE_INDEX]);
                string rule = data[RULE_INDEX];
                string name = data[NAME_INDEX];
                float komi = float.Parse(data[KOMI_INDEX], CultureInfo.InvariantCulture.NumberFormat);
                int handicap = int.Parse(data[HANDICAP_INDEX]);
                string colorHandicap = data[COLOR_HANDICAP_INDEX];
                int id = Server.CustomGames.Count + 1; 
                GameConfiguration config = new GameConfiguration(size, rule, komi, name, handicap, colorHandicap);
                Game newGame = GameFactory.CreateCustomGame(config);
                newGame.AddPlayer(player);
                Server.CustomGames[id] = newGame;
                availableGameManager.InsertAvailableGame(newGame); // Ajout de la partie dans le dictionnaire des parties
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
                Server.MatchmakingGames[id].Player1 = player;
                response = $"{id}-"; // Renvoi del'id de la partie créée
                type = "Send_";
            }
        }
    }
}
