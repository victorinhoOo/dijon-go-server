using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;
using WebSocket;
using System.Net.Sockets;
using GoLogic;

namespace Tests.WebSocket
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

        [Fact]
        public void Test_StringifyingWithKo()
        {
            String test = "x,y,color\r\n0,0,Empty\r\n0,1,Empty\r\n0,2,Empty\r\n0,3,Empty\r\n0,4,Empty\r\n0,5,Empty\r\n0,6,Empty\r\n0,7,Empty\r\n0,8,Empty\r\n0,9,Empty\r\n0,10,Empty\r\n0,11,Empty\r\n0,12,Empty\r\n0,13,Empty\r\n0,14,Empty\r\n0,15,Empty\r\n0,16,Empty\r\n0,17,Empty\r\n0,18,Empty\r\n1,0,Empty\r\n1,1,Empty\r\n1,2,Black\r\n1,3,White\r\n1,4,Empty\r\n1,5,Empty\r\n1,6,Empty\r\n1,7,Empty\r\n1,8,Empty\r\n1,9,Empty\r\n1,10,Empty\r\n1,11,Empty\r\n1,12,Empty\r\n1,13,Empty\r\n1,14,Empty\r\n1,15,Empty\r\n1,16,Empty\r\n1,17,Empty\r\n1,18,Empty\r\n2,0,Empty\r\n2,1,Black\r\n2,2,Ko\r\n2,3,Black\r\n2,4,White\r\n2,5,Empty\r\n2,6,Empty\r\n2,7,Empty\r\n2,8,Empty\r\n2,9,Empty\r\n2,10,Empty\r\n2,11,Empty\r\n2,12,Empty\r\n2,13,Empty\r\n2,14,Empty\r\n2,15,Empty\r\n2,16,Empty\r\n2,17,Empty\r\n2,18,Empty\r\n3,0,Empty\r\n3,1,Empty\r\n3,2,Black\r\n3,3,White\r\n3,4,Empty\r\n3,5,Empty\r\n3,6,Empty\r\n3,7,Empty\r\n3,8,Empty\r\n3,9,Empty\r\n3,10,Empty\r\n3,11,Empty\r\n3,12,Empty\r\n3,13,Empty\r\n3,14,Empty\r\n3,15,Empty\r\n3,16,Empty\r\n3,17,Empty\r\n3,18,Empty\r\n4,0,Empty\r\n4,1,Empty\r\n4,2,Empty\r\n4,3,Empty\r\n4,4,Empty\r\n4,5,Empty\r\n4,6,Empty\r\n4,7,Empty\r\n4,8,Empty\r\n4,9,Empty\r\n4,10,Empty\r\n4,11,Empty\r\n4,12,Empty\r\n4,13,Empty\r\n4,14,Empty\r\n4,15,Empty\r\n4,16,Empty\r\n4,17,Empty\r\n4,18,Empty\r\n5,0,Empty\r\n5,1,Empty\r\n5,2,Empty\r\n5,3,Empty\r\n5,4,Empty\r\n5,5,Empty\r\n5,6,Empty\r\n5,7,Empty\r\n5,8,Empty\r\n5,9,Empty\r\n5,10,Empty\r\n5,11,Empty\r\n5,12,Empty\r\n5,13,Empty\r\n5,14,Empty\r\n5,15,Empty\r\n5,16,Empty\r\n5,17,Empty\r\n5,18,Empty\r\n6,0,Empty\r\n6,1,Empty\r\n6,2,Empty\r\n6,3,Empty\r\n6,4,Empty\r\n6,5,Empty\r\n6,6,Empty\r\n6,7,Empty\r\n6,8,Empty\r\n6,9,Empty\r\n6,10,Empty\r\n6,11,Empty\r\n6,12,Empty\r\n6,13,Empty\r\n6,14,Empty\r\n6,15,Empty\r\n6,16,Empty\r\n6,17,Empty\r\n6,18,Empty\r\n7,0,Empty\r\n7,1,Empty\r\n7,2,Empty\r\n7,3,Empty\r\n7,4,Empty\r\n7,5,Empty\r\n7,6,Empty\r\n7,7,Empty\r\n7,8,Empty\r\n7,9,Empty\r\n7,10,Empty\r\n7,11,Empty\r\n7,12,Empty\r\n7,13,Empty\r\n7,14,Empty\r\n7,15,Empty\r\n7,16,Empty\r\n7,17,Empty\r\n7,18,Empty\r\n8,0,Empty\r\n8,1,Empty\r\n8,2,Empty\r\n8,3,Black\r\n8,4,Empty\r\n8,5,Empty\r\n8,6,Empty\r\n8,7,Empty\r\n8,8,Empty\r\n8,9,Empty\r\n8,10,Empty\r\n8,11,Empty\r\n8,12,Empty\r\n8,13,Empty\r\n8,14,Empty\r\n8,15,Empty\r\n8,16,Empty\r\n8,17,Empty\r\n8,18,Empty\r\n9,0,Empty\r\n9,1,Empty\r\n9,2,Empty\r\n9,3,Empty\r\n9,4,Empty\r\n9,5,Empty\r\n9,6,Empty\r\n9,7,Empty\r\n9,8,Empty\r\n9,9,Empty\r\n9,10,Empty\r\n9,11,Empty\r\n9,12,Empty\r\n9,13,Empty\r\n9,14,Empty\r\n9,15,Empty\r\n9,16,Empty\r\n9,17,Empty\r\n9,18,Empty\r\n10,0,Empty\r\n10,1,Empty\r\n10,2,Empty\r\n10,3,Empty\r\n10,4,Empty\r\n10,5,Empty\r\n10,6,Empty\r\n10,7,Empty\r\n10,8,Empty\r\n10,9,Empty\r\n10,10,Empty\r\n10,11,Empty\r\n10,12,Empty\r\n10,13,Empty\r\n10,14,Empty\r\n10,15,Empty\r\n10,16,Empty\r\n10,17,Empty\r\n10,18,Empty\r\n11,0,Empty\r\n11,1,Empty\r\n11,2,Empty\r\n11,3,Empty\r\n11,4,Empty\r\n11,5,Empty\r\n11,6,Empty\r\n11,7,Empty\r\n11,8,Empty\r\n11,9,Empty\r\n11,10,Empty\r\n11,11,Empty\r\n11,12,Empty\r\n11,13,Empty\r\n11,14,Empty\r\n11,15,Empty\r\n11,16,Empty\r\n11,17,Empty\r\n11,18,Empty\r\n12,0,Empty\r\n12,1,Empty\r\n12,2,Empty\r\n12,3,Empty\r\n12,4,Empty\r\n12,5,Empty\r\n12,6,Empty\r\n12,7,Empty\r\n12,8,Empty\r\n12,9,Empty\r\n12,10,Empty\r\n12,11,Empty\r\n12,12,Empty\r\n12,13,Empty\r\n12,14,Empty\r\n12,15,Empty\r\n12,16,Empty\r\n12,17,Empty\r\n12,18,Empty\r\n13,0,Empty\r\n13,1,Empty\r\n13,2,Empty\r\n13,3,Empty\r\n13,4,Empty\r\n13,5,Empty\r\n13,6,Empty\r\n13,7,Empty\r\n13,8,Empty\r\n13,9,Empty\r\n13,10,Empty\r\n13,11,Empty\r\n13,12,Empty\r\n13,13,Empty\r\n13,14,Empty\r\n13,15,Empty\r\n13,16,Empty\r\n13,17,Empty\r\n13,18,Empty\r\n14,0,Empty\r\n14,1,Empty\r\n14,2,Empty\r\n14,3,Empty\r\n14,4,Empty\r\n14,5,Empty\r\n14,6,Empty\r\n14,7,Empty\r\n14,8,Empty\r\n14,9,Empty\r\n14,10,Empty\r\n14,11,Empty\r\n14,12,Empty\r\n14,13,Empty\r\n14,14,Empty\r\n14,15,Empty\r\n14,16,Empty\r\n14,17,Empty\r\n14,18,Empty\r\n15,0,Empty\r\n15,1,Empty\r\n15,2,Empty\r\n15,3,Empty\r\n15,4,Empty\r\n15,5,Empty\r\n15,6,Empty\r\n15,7,Empty\r\n15,8,Empty\r\n15,9,Empty\r\n15,10,Empty\r\n15,11,Empty\r\n15,12,Empty\r\n15,13,Empty\r\n15,14,Empty\r\n15,15,Empty\r\n15,16,Empty\r\n15,17,Empty\r\n15,18,Empty\r\n16,0,Empty\r\n16,1,Empty\r\n16,2,Empty\r\n16,3,Empty\r\n16,4,Empty\r\n16,5,Empty\r\n16,6,Empty\r\n16,7,Empty\r\n16,8,Empty\r\n16,9,Empty\r\n16,10,Empty\r\n16,11,Empty\r\n16,12,Empty\r\n16,13,Empty\r\n16,14,Empty\r\n16,15,Empty\r\n16,16,Empty\r\n16,17,Empty\r\n16,18,Empty\r\n17,0,Empty\r\n17,1,Empty\r\n17,2,Empty\r\n17,3,Empty\r\n17,4,Empty\r\n17,5,Empty\r\n17,6,Empty\r\n17,7,Empty\r\n17,8,Empty\r\n17,9,Empty\r\n17,10,Empty\r\n17,11,Empty\r\n17,12,Empty\r\n17,13,Empty\r\n17,14,Empty\r\n17,15,Empty\r\n17,16,Empty\r\n17,17,Empty\r\n17,18,Empty\r\n18,0,Empty\r\n18,1,Empty\r\n18,2,Empty\r\n18,3,Empty\r\n18,4,Empty\r\n18,5,Empty\r\n18,6,Empty\r\n18,7,Empty\r\n18,8,Empty\r\n18,9,Empty\r\n18,10,Empty\r\n18,11,Empty\r\n18,12,Empty\r\n18,13,Empty\r\n18,14,Empty\r\n18,15,Empty\r\n18,16,Empty\r\n18,17,Empty\r\n18,18,Empty\r\n";
            
            Game game = new Game(19,"j");

            PlayKoSituation(game);
            string mess = game.StringifyGameBoard();
            test = test.Replace("\r\n", ";");
            mess = mess.Replace("\n", ";");
            Assert.Equal(test, mess);
            
        }
        
    }
}
