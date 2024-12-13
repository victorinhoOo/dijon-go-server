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
    /// Représente la stratégie d'annulation d'une partie
    /// </summary>
    public class CancelStrategy : IStrategy
    {
        /// <summary>
        /// Exécute la stratégie d'annulation d'une partie
        /// </summary>
        /// <param name="player">Le joueur qui annule la partie</param>
        /// <param name="data"></param>
        /// <param name="gameType"></param>
        /// <param name="response"></param>
        /// <param name="type"></param>
        public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type)
        {
            if(gameType == GameType.MATCHMAKING)
            {
                string id = data[0];
                response = $"{id}-Cancelled";
                type = "Broadcast_";
                int intId = Convert.ToInt16(id);

                Server.MatchmakingGames.TryRemove(intId, out _);
            }

        }
    }
}
