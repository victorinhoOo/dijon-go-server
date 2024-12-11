using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;
using WebSocket.Strategy.Enumerations;

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
        private const int ID_GAME = 0;

        /// <summary>
        /// Exécute l'action de passer le tour
        /// </summary>
        /// <param name="player">Le joueur qui souhaite passer son tour</param>
        /// <param name="data">Tableau contenant l'ID de la partie</param>
        /// <param name="gameType">Type de partie ("custom" ou "matchmaking")</param>
        /// <param name="response">Message de réponse à renvoyer (modifié par référence)</param>
        /// <param name="type">Type de réponse à envoyer (modifié par référence)</param>
        public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type)
        {
            string stringId = data[ID_GAME];
            int idGame = Convert.ToInt16(stringId);
            Game game = null;
            if (gameType == GameType.CUSTOM)
            {
                game = Server.CustomGames[idGame];
            }
            else if (gameType == GameType.MATCHMAKING)
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
