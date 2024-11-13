using GoLogic;
using GoLogic.Score;

namespace Tests.Test_GoLogic
{
    public class ScoreTest
    {
        [Fact]
        public void ScoreRule_Chinese()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(2, 1); // noir
            gameLogic.PlaceStone(2, 4); // blanc
            gameLogic.PlaceStone(3, 2); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(8, 3); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(2, 3); // noir capture blanc en (2, 2)

            // . . . . . . . . .
            // . . @ O . . . . .
            // . @ . @ O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .
            // . : vide, @ : noir, O : blanc

            (int black, int white) = gameScore.CalculateScore();

            Assert.Equal(6, black);
            Assert.Equal(3, white);
        }

        [Fact]
        public void ScoreRule_Japanese()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new JapaneseScoreRule(gameBoard);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(2, 1); // noir
            gameLogic.PlaceStone(2, 4); // blanc
            gameLogic.PlaceStone(3, 2); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(8, 3); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(2, 3); // noir capture blanc en (2, 2)

            // . . . . . . . . .
            // . . @ O . . . . .
            // . @ . @ O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .
            // . : vide, @ : noir, O : blanc

            (int black, int white) = gameScore.CalculateScore();
            
            Assert.Equal(2, black);
            Assert.Equal(0, white);
        }

        [Fact]
        public void ScoreRule_Chinese2()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 1); // noir
            gameLogic.PlaceStone(0, 2); // blanc
            gameLogic.PlaceStone(1, 1); // noir
            gameLogic.PlaceStone(1, 2); // blanc
            gameLogic.PlaceStone(1, 0); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(8, 0); // noir
            gameLogic.PlaceStone(2, 1); // blanc
            gameLogic.PlaceStone(8, 1); // noir
            gameLogic.PlaceStone(2, 0); // blanc

            // . @ O . . . . . .
            // @ @ O . . . . . .
            // O O O . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // @ @ . . . . . . .
            // . : vide, @ : noir, O : blanc

            (int black, int white) = gameScore.CalculateScore();

            // TODO: décommenter ou retirer
            //Assert.Equal(9, black);
            //Assert.Equal(2, white);
        }

        [Fact]
        public void ScoreRule_One_Eye_capture()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 3); // noir
            gameLogic.PlaceStone(0, 2); // blanc
            gameLogic.PlaceStone(0, 5); // noir
            gameLogic.PlaceStone(0, 6); // blanc
            gameLogic.PlaceStone(1, 3); // noir
            gameLogic.PlaceStone(1, 2); // blanc
            gameLogic.PlaceStone(1, 4); // noir
            gameLogic.PlaceStone(1, 6); // blanc
            gameLogic.PlaceStone(1, 5); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(8, 0); // noir
            gameLogic.PlaceStone(2, 3); // blanc
            gameLogic.PlaceStone(8, 1); // noir
            gameLogic.PlaceStone(2, 4); // blanc
            gameLogic.PlaceStone(8, 2); // noir
            gameLogic.PlaceStone(2, 5); // blanc
            gameLogic.PlaceStone(8, 3); // noir
            gameLogic.PlaceStone(2, 6); // blanc
            gameLogic.PlaceStone(8, 4); // noir

            // Capture du groupe de pierres blanches en plaçant un noir en (0, 4)
            gameLogic.PlaceStone(0, 4);

            // . . O @ . @ O . .
            // . . O @ @ @ O . .
            // . . O O O O O . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . : vide, @ : noir, O : blanc

            Assert.Equal(StoneColor.White, gameBoard.GetStone(0, 4).Color);
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(1, 4).Color);
        }

        [Fact]
        public void ScoreRule_Two_Eyes_Nocapture()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 4); // noir
            gameLogic.PlaceStone(0, 3); // blanc
            gameLogic.PlaceStone(0, 6); // noir
            gameLogic.PlaceStone(0, 7); // blanc
            gameLogic.PlaceStone(1, 4); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(1, 5); // noir
            gameLogic.PlaceStone(1, 7); // blanc
            gameLogic.PlaceStone(1, 6); // noir
            gameLogic.PlaceStone(2, 3); // blanc
            gameLogic.PlaceStone(2, 4); // noir
            gameLogic.PlaceStone(2, 7); // blanc
            gameLogic.PlaceStone(2, 6); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(3, 4); // noir
            gameLogic.PlaceStone(3, 7); // blanc
            gameLogic.PlaceStone(3, 5); // noir
            gameLogic.PlaceStone(4, 3); // blanc
            gameLogic.PlaceStone(3, 6); // noir
            gameLogic.PlaceStone(4, 4); // blanc
            gameLogic.PlaceStone(8, 0); // noir
            gameLogic.PlaceStone(4, 5); // blanc
            gameLogic.PlaceStone(8, 1); // noir
            gameLogic.PlaceStone(4, 6); // blanc
            gameLogic.PlaceStone(8, 2); // noir
            gameLogic.PlaceStone(4, 7); // blanc
            gameLogic.PlaceStone(8, 3); // noir

            // blanc n'est pas placer en (0, 5) car c'est un coup suicide
            Assert.Throws<InvalidOperationException>(() => gameLogic.PlaceStone(0, 5));

            // . . . O @ . @ O .
            // . . . O @ @ @ O .
            // . . . O @ . @ O .
            // . . . O @ @ @ O .
            // . . . O O O O O .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . : vide, @ : noir, O : blanc
        }

        [Fact] //TODO: !!! Penser à le retier
        public void No361bug()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(8, 8);
            (int black, int white) = gameScore.CalculateScore();

            Assert.Equal(1, black);
            Assert.Equal(0, white);
        }
    }
}
