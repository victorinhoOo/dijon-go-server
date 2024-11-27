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
    /// le joueur place une pierre sur le plateau
    /// </summary>
    public class PlaceStoneStrategy : IStrategy
    {
        public void execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            if (idGame != 0)
            {
                Game game = null;
                if (gameType == "custom")
                {
                    game = Server.Games[idGame];
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
                        response = $"{idGame}-{game.StringifyGameBoard()}-{capturedBlackStones}-{capturedWhiteStones}-{timeRemaining.Split(',')[0]}";
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
