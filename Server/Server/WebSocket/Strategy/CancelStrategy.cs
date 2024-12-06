using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;

namespace WebSocket.Strategy
{
    public class CancelStrategy : IStrategy
    {
        public void Execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string id = data[0];
            response = $"{id}-Cancelled";
            type = "Broadcast_";
            int intId = Convert.ToInt16(id);

            Server.MatchmakingGames.TryRemove(intId, out _);
        }
    }
}
