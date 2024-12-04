using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie permettant à un joueur de passer son tour dans une partie
    /// </summary>
    /// <remarks>
    /// Cette stratégie vérifie :
    /// - Si c'est bien le tour du joueur
    /// - Le type de partie (personnalisée ou matchmaking)
    /// </remarks>
    public class SkipStrategy : IStrategy
    {
        /// <summary>
        /// Exécute l'action de passer le tour
        /// </summary>
        /// <param name="player">Le joueur qui souhaite passer son tour</param>
        /// <param name="data">Tableau contenant l'ID de la partie</param>
        /// <param name="gameType">Type de partie ("custom" ou "matchmaking")</param>
        /// <param name="response">Message de réponse à renvoyer (modifié par référence)</param>
        /// <param name="type">Type de réponse à envoyer (modifié par référence)</param>
        public void Execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            Game game = null;
            if (gameType == "custom")
            {
                game = Server.CustomGames[idGame];
            }
            else if (gameType == "matchmaking")
            {
                game = Server.MatchmakingGames[idGame];
            }
            if (player == game.CurrentTurn)
            {
                game.SkipTurn();
                type = "Broadcast_";
                response = $"{idGame}-Skipped";
            }
            else
            {
                response = $"{idGame}-Not your turn";
                type = "Send_";
            }
        }
    }
}
