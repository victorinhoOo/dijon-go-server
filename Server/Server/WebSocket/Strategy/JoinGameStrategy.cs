using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model.Managers;
using WebSocket.Strategy.Enumerations;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie permettant à un joueur de rejoindre une partie existante
    /// </summary>
    public class JoinGameStrategy : IStrategy
    {
        private const int GAMEID_INDEX = 0;

        private AvailableGameManager availableGameManager;
        public JoinGameStrategy()
        {
            this.availableGameManager = new AvailableGameManager();
        }

        /// <summary>
        /// Exécute la logique permettant à un joueur de rejoindre une partie
        /// </summary>
        /// <param name="player">Le client représentant le joueur qui rejoint la partie</param>
        /// <param name="data">Tableau contenant les données nécessaires [idPartie, token]</param>
        /// <param name="gameType">Type de partie ("custom" ou "matchmaking")</param>
        /// <param name="response">Réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Type de réponse à envoyer (modifié par référence)</param>
        public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type)
        {
            string stringIdGame = data[GAMEID_INDEX];
            int idGame = Convert.ToInt16(stringIdGame);
            if (gameType == GameType.CUSTOM)
            {
                Server.CustomGames[idGame].AddPlayer(player); // Ajout du client en tant que joueur 2
                this.availableGameManager.DeleteAvailableGame(idGame); // Suppression de la partie de la liste des parties disponibles
                response = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
            else if (gameType == GameType.MATCHMAKING)
            {
                Server.MatchmakingGames[idGame].AddPlayer(player);
                response = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
        }
    }
}
