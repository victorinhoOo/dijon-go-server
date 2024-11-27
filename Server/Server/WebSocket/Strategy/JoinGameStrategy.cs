using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie permettant à un joueur de rejoindre une partie existante
    /// </summary>
    public class JoinGameStrategy : IStrategy
    {

        private IGameDAO gameDAO;
        public JoinGameStrategy()
        {
            this.gameDAO = new GameDAO();
        }

        /// <summary>
        /// Exécute la logique permettant à un joueur de rejoindre une partie
        /// </summary>
        /// <param name="player">Le client représentant le joueur qui rejoint la partie</param>
        /// <param name="data">Tableau contenant les données nécessaires [idPartie, token]</param>
        /// <param name="gameType">Type de partie ("custom" ou "matchmaking")</param>
        /// <param name="response">Réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Type de réponse à envoyer (modifié par référence)</param>
        public void Execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            if (gameType == "custom")
            {

                player.User.Token = data[2]; // Récupération du token du joueur afin d'afficher son pseudo et sa photo de profil
                Server.CustomGames[idGame].AddPlayer(player); // Ajout du client en tant que joueur 2
                this.gameDAO.DeleteGame(idGame); // Suppression de la partie de la liste des parties disponibles
                response = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
            else if (gameType == "matchmaking")
            {
                player.User.Token = data[2];
                Server.MatchmakingGames[idGame].AddPlayer(player);
                response = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
        }
    }
}
