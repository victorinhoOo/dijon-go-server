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
    /// Le joueur passe son tour
    /// </summary>
    public class SkipStrategy : IStrategy
    {
        public void execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            Game game = null;
            if (gameType == "custom")
            {
                game = Server.Games[idGame];
            }
            else if (gameType == "matchmaking")
            {
                game = Server.MatchmakingGames[idGame];
            }
            if (gameType == "matchmaking")
            {
                game = Server.MatchmakingGames[idGame];
            }
            if (player == game.CurrentTurn)
            {
                game.SkipTurn();
                type = "Broadcast_";
                response = $"{idGame}-Turn skipped";
            }
            else
            {
                response = $"{idGame}-Not your turn";
                type = "Send_";
            }
        }
    }
}
