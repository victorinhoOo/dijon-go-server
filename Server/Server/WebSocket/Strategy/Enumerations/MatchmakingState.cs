using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Strategy.Enumerations
{
    /// <summary>
    /// Représente l'état du matchmaking (OK, RETRY, TIMEOUT)
    /// </summary>
    public enum MatchmakingState
    {
        OK,
        RETRY,
        TIMEOUT
    }
}
