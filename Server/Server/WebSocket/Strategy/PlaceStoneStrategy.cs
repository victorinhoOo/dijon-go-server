using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;
using WebSocket.Strategy.Enumerations;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie permettant à un joueur de placer une pierre sur le plateau.
    /// </summary>
    public class PlaceStoneStrategy : IStrategy
    {

        private const int GAME_ID = 0;
        private const int STONEPLACED_X = 2;
        private const int STONEPLACED_Y = 3;

        /// <summary>
        /// Exécute la stratégie de placement d'une pierre.
        /// </summary>
        /// <param name="player">Le client (joueur) qui effectue l'action</param>
        /// <param name="data">Tableau contenant les données de l'action [idGame, x, y]</param>
        /// <param name="gameType">Type de partie ("custom" ou "matchmaking")</param>
        /// <param name="response">Message de réponse à envoyer aux clients (modifié par référence)</param>
        /// <param name="type">Type de réponse ("Broadcast_" ou "Send_") (modifié par référence)</param>
        public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type)
        {
            string stringGameId = data[GAME_ID];
            int idGame = Convert.ToInt16(stringGameId);
            if (idGame != 0)
            {
                Game game = null;
                if (gameType == GameType.CUSTOM)
                {
                    game = Server.CustomGames[idGame];
                }
                else if (gameType == GameType.MATCHMAKING)
                {
                    game = Server.MatchmakingGames[idGame];
                }
                if (game.CurrentTurn == player) // si c'est le tour du joueur
                {
                    try
                    {
                        int x = Convert.ToInt32(data[STONEPLACED_X]);
                        int y = Convert.ToInt32(data[STONEPLACED_Y]);

                        string timeRemaining = game.PlaceStone(x, y).Result;
                        (int capturedBlackStones, int capturedWhiteStones) = game.GetCapturedStone();
                        game.ChangeTurn();
                        response = $"{idGame}-Stone-{game.StringifyGameBoard()}-{capturedBlackStones}-{capturedWhiteStones}-{timeRemaining}";
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
