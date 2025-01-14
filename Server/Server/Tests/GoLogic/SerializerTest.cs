using GoLogic.Goban;
using GoLogic;
using GoLogic.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;

namespace Tests.GoLogic
{
    public class SerializerTest
    {
        /// <summary>
        /// Ne fonctionne que sur la pipeline
        /// A cause de la gestion des \n de gitHub
        /// </summary>
        [Fact]
        public void Test_SerialisationWithKo()
        {
            String test = "x,y,color!0,0,Empty!0,1,Empty!0,2,Empty!0,3,Empty!0,4,Empty!0,5,Empty!0,6,Empty!0,7,Empty!0,8,Empty!0,9,Empty!0,10,Empty!0,11,Empty!0,12,Empty!0,13,Empty!0,14,Empty!0,15,Empty!0,16,Empty!0,17,Empty!0,18,Empty!1,0,Empty!1,1,Empty!1,2,Black!1,3,White!1,4,Empty!1,5,Empty!1,6,Empty!1,7,Empty!1,8,Empty!1,9,Empty!1,10,Empty!1,11,Empty!1,12,Empty!1,13,Empty!1,14,Empty!1,15,Empty!1,16,Empty!1,17,Empty!1,18,Empty!2,0,Empty!2,1,Black!2,2,Ko!2,3,Black!2,4,White!2,5,Empty!2,6,Empty!2,7,Empty!2,8,Empty!2,9,Empty!2,10,Empty!2,11,Empty!2,12,Empty!2,13,Empty!2,14,Empty!2,15,Empty!2,16,Empty!2,17,Empty!2,18,Empty!3,0,Empty!3,1,Empty!3,2,Black!3,3,White!3,4,Empty!3,5,Empty!3,6,Empty!3,7,Empty!3,8,Empty!3,9,Empty!3,10,Empty!3,11,Empty!3,12,Empty!3,13,Empty!3,14,Empty!3,15,Empty!3,16,Empty!3,17,Empty!3,18,Empty!4,0,Empty!4,1,Empty!4,2,Empty!4,3,Empty!4,4,Empty!4,5,Empty!4,6,Empty!4,7,Empty!4,8,Empty!4,9,Empty!4,10,Empty!4,11,Empty!4,12,Empty!4,13,Empty!4,14,Empty!4,15,Empty!4,16,Empty!4,17,Empty!4,18,Empty!5,0,Empty!5,1,Empty!5,2,Empty!5,3,Empty!5,4,Empty!5,5,Empty!5,6,Empty!5,7,Empty!5,8,Empty!5,9,Empty!5,10,Empty!5,11,Empty!5,12,Empty!5,13,Empty!5,14,Empty!5,15,Empty!5,16,Empty!5,17,Empty!5,18,Empty!6,0,Empty!6,1,Empty!6,2,Empty!6,3,Empty!6,4,Empty!6,5,Empty!6,6,Empty!6,7,Empty!6,8,Empty!6,9,Empty!6,10,Empty!6,11,Empty!6,12,Empty!6,13,Empty!6,14,Empty!6,15,Empty!6,16,Empty!6,17,Empty!6,18,Empty!7,0,Empty!7,1,Empty!7,2,Empty!7,3,Empty!7,4,Empty!7,5,Empty!7,6,Empty!7,7,Empty!7,8,Empty!7,9,Empty!7,10,Empty!7,11,Empty!7,12,Empty!7,13,Empty!7,14,Empty!7,15,Empty!7,16,Empty!7,17,Empty!7,18,Empty!8,0,Empty!8,1,Empty!8,2,Empty!8,3,Black!8,4,Empty!8,5,Empty!8,6,Empty!8,7,Empty!8,8,Empty!8,9,Empty!8,10,Empty!8,11,Empty!8,12,Empty!8,13,Empty!8,14,Empty!8,15,Empty!8,16,Empty!8,17,Empty!8,18,Empty!9,0,Empty!9,1,Empty!9,2,Empty!9,3,Empty!9,4,Empty!9,5,Empty!9,6,Empty!9,7,Empty!9,8,Empty!9,9,Empty!9,10,Empty!9,11,Empty!9,12,Empty!9,13,Empty!9,14,Empty!9,15,Empty!9,16,Empty!9,17,Empty!9,18,Empty!10,0,Empty!10,1,Empty!10,2,Empty!10,3,Empty!10,4,Empty!10,5,Empty!10,6,Empty!10,7,Empty!10,8,Empty!10,9,Empty!10,10,Empty!10,11,Empty!10,12,Empty!10,13,Empty!10,14,Empty!10,15,Empty!10,16,Empty!10,17,Empty!10,18,Empty!11,0,Empty!11,1,Empty!11,2,Empty!11,3,Empty!11,4,Empty!11,5,Empty!11,6,Empty!11,7,Empty!11,8,Empty!11,9,Empty!11,10,Empty!11,11,Empty!11,12,Empty!11,13,Empty!11,14,Empty!11,15,Empty!11,16,Empty!11,17,Empty!11,18,Empty!12,0,Empty!12,1,Empty!12,2,Empty!12,3,Empty!12,4,Empty!12,5,Empty!12,6,Empty!12,7,Empty!12,8,Empty!12,9,Empty!12,10,Empty!12,11,Empty!12,12,Empty!12,13,Empty!12,14,Empty!12,15,Empty!12,16,Empty!12,17,Empty!12,18,Empty!13,0,Empty!13,1,Empty!13,2,Empty!13,3,Empty!13,4,Empty!13,5,Empty!13,6,Empty!13,7,Empty!13,8,Empty!13,9,Empty!13,10,Empty!13,11,Empty!13,12,Empty!13,13,Empty!13,14,Empty!13,15,Empty!13,16,Empty!13,17,Empty!13,18,Empty!14,0,Empty!14,1,Empty!14,2,Empty!14,3,Empty!14,4,Empty!14,5,Empty!14,6,Empty!14,7,Empty!14,8,Empty!14,9,Empty!14,10,Empty!14,11,Empty!14,12,Empty!14,13,Empty!14,14,Empty!14,15,Empty!14,16,Empty!14,17,Empty!14,18,Empty!15,0,Empty!15,1,Empty!15,2,Empty!15,3,Empty!15,4,Empty!15,5,Empty!15,6,Empty!15,7,Empty!15,8,Empty!15,9,Empty!15,10,Empty!15,11,Empty!15,12,Empty!15,13,Empty!15,14,Empty!15,15,Empty!15,16,Empty!15,17,Empty!15,18,Empty!16,0,Empty!16,1,Empty!16,2,Empty!16,3,Empty!16,4,Empty!16,5,Empty!16,6,Empty!16,7,Empty!16,8,Empty!16,9,Empty!16,10,Empty!16,11,Empty!16,12,Empty!16,13,Empty!16,14,Empty!16,15,Empty!16,16,Empty!16,17,Empty!16,18,Empty!17,0,Empty!17,1,Empty!17,2,Empty!17,3,Empty!17,4,Empty!17,5,Empty!17,6,Empty!17,7,Empty!17,8,Empty!17,9,Empty!17,10,Empty!17,11,Empty!17,12,Empty!17,13,Empty!17,14,Empty!17,15,Empty!17,16,Empty!17,17,Empty!17,18,Empty!18,0,Empty!18,1,Empty!18,2,Empty!18,3,Empty!18,4,Empty!18,5,Empty!18,6,Empty!18,7,Empty!18,8,Empty!18,9,Empty!18,10,Empty!18,11,Empty!18,12,Empty!18,13,Empty!18,14,Empty!18,15,Empty!18,16,Empty!18,17,Empty!18,18,Empty!";
            var gameBoard = new GameBoard(19);
            var gameLogic = new GameLogic(gameBoard);
            var boardSerializer = new BoardSerializer(gameLogic);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(2, 1); // noir
            gameLogic.PlaceStone(2, 4); // blanc
            gameLogic.PlaceStone(3, 2); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(8, 3); // noir
            gameLogic.PlaceStone(1, 3); // blanc

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

            gameLogic.PlaceStone(2, 3); // noir capture blanc en (2, 2)

            string mess = boardSerializer.StringifyGoban(gameLogic.CurrentTurn);
            Assert.Equal(test, mess);
        }
    }
}
