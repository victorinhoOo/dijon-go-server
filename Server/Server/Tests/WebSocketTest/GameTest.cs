using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;
using WebSocket;
using System.Net.Sockets;
using GoLogic;
using DotNetEnv;

namespace Tests.WebSockets
{
    public class GameTest
    {
        private void PlayKoSituation(Game game)
        {
            game.PlaceStone(1, 2); // noir
            game.PlaceStone(2, 2); // blanc
            game.PlaceStone(2, 1); // noir
            game.PlaceStone(2, 4); // blanc
            game.PlaceStone(3, 2); // noir
            game.PlaceStone(3, 3); // blanc
            game.PlaceStone(8, 3); // noir
            game.PlaceStone(1, 3); // blanc

            // . : vide, @ : noir, O : blanc
            // . . . . . . . . .
            // . . @ O . . . . .
            // . @ O . O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .

            game.PlaceStone(2, 3); // noir capture blanc en (2, 2)

            // . : vide, @ : noir, O : blanc, + : ko
            // . . . . . . . . .
            // . . @ O . . . . .
            // . @ + @ O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .

        }
    }
}
