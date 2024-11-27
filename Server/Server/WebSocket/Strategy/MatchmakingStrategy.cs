using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Le joueur se met en recherche de partie
    /// </summary>
    public class MatchmakingStrategy : IStrategy
    {
        public void execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            Server.WaitingPlayers.Enqueue(player);
            int nbMatchmakingGames = Server.MatchmakingGames.Count();
            DateTime startTime = DateTime.Now;
            const int TIMEOUT_SECONDS = 5;

            Client player1 = Server.WaitingPlayers.Peek();
            if (player == player1)
            {
                // Le premier joueur attend avec un délai qui permet de vérifier périodiquement
                // si un second joueur est arrivé
                while (Server.WaitingPlayers.Count < 2)
                {
                    if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                    {
                        Server.WaitingPlayers.Dequeue(); // Retire le joueur de la file d'attente
                        response = "0-Timeout";
                        type = "Send_";
                        return;
                    }
                    Thread.Sleep(100);
                }
                response = "0-Create-matchmaking";
                type = "Send_";
            }
            else
            {
                // Le deuxième joueur a rejoint la file
                while (Server.MatchmakingGames.Count == nbMatchmakingGames)
                {
                    if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                    {
                        Server.WaitingPlayers.Dequeue(); // Retire le joueur de la file d'attente
                        response = "0-Timeout";
                        type = "Send_";
                        return;
                    }
                    Thread.Sleep(100);
                }
                Server.WaitingPlayers.Dequeue();
                Server.WaitingPlayers.Dequeue();
                string idGame = (Server.MatchmakingGames.Count()).ToString();
                response = $"{idGame}-Join-matchmaking";
                type = "Send_";
            }
        }
    }
}
