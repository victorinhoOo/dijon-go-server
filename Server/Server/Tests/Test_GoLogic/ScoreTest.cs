using Go_logic_csharp;

namespace Test_GoLogic
{
    public class ScoreTest
    {
        [Fact]
        public void Score_CalculateChinese()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);
            var gameScore = new ScoreCalculator(gameBoard);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(2, 1); // noir
            gameLogic.PlaceStone(2, 4); // blanc
            gameLogic.PlaceStone(3, 2); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(8, 3); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(2, 3); // noir capture blanc en (2, 2)

            // . : vide, @ : noir, O : blanc
            // . . . . . . . . .
            // . . @ O . . . . .
            // . @ . @ O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .

            (int black, int white) = gameScore.CalculateScoreChinese();
            
            Assert.Equal(6, black);
            Assert.Equal(3, white);
        }

        [Fact]
        public void Score_CalculateJapanese()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);
            var gameScore = new ScoreCalculator(gameBoard);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(2, 1); // noir
            gameLogic.PlaceStone(2, 4); // blanc
            gameLogic.PlaceStone(3, 2); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(8, 3); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(2, 3); // noir capture blanc en (2, 2)

            // . : vide, @ : noir, O : blanc
            // . . . . . . . . .
            // . . @ O . . . . .
            // . @ . @ O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .

            (int black, int white) = gameScore.CalculateScoreJapanese();

            Assert.Equal(2, black);
            Assert.Equal(0, white);
        }
    }
}
