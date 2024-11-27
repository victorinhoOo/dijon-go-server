using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie permettant à un joueur de placer une pierre sur le plateau.
    /// </summary>
    public class PlaceStoneStrategy : IStrategy
    {
        /// <summary>
        /// Exécute la stratégie de placement d'une pierre.
        /// </summary>
        /// <param name="player">Le client (joueur) qui effectue l'action</param>
        /// <param name="data">Tableau contenant les données de l'action [idGame, x, y]</param>
        /// <param name="gameType">Type de partie ("custom" ou "matchmaking")</param>
        /// <param name="response">Message de réponse à envoyer aux clients (modifié par référence)</param>
        /// <param name="type">Type de réponse ("Broadcast_" ou "Send_") (modifié par référence)</param>
        public void Execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            if (idGame != 0)
            {
                Game game = null;
                if (gameType == "custom")
                {
                    game = Server.CustomGames[idGame];
                }
                else if (gameType == "matchmaking")
                {
                    game = Server.MatchmakingGames[idGame];
                }
                if (game.CurrentTurn == player) // si c'est le tour du joueur
                {
                    try
                    {
                        int x = Convert.ToInt32(data[2]);
                        int y = Convert.ToInt32(data[3]);

                        string timeRemaining = game.PlaceStone(x, y); // pose de la pierre
                        (int capturedBlackStones, int capturedWhiteStones) = game.GetCapturedStone(); // récupération des pierres capturées
                        game.ChangeTurn(); // changement de tour
                        response = $"{idGame}-Stone-{game.StringifyGameBoard()}-{capturedBlackStones}-{capturedWhiteStones}-{timeRemaining.Split(',')[0]}";
                        type = "Broadcast_";
                    }
                    catch (Exception e)
                    {
                        response = $"{idGame}-Error-{e.Message}";
                        type = "Send_";
                    }
                }
                else // si ce n'est pas le tour du joueur
                {
                    response = $"{idGame}-Not your turn";
                    type = "Send_";
                }
            }
        }
    }
}
