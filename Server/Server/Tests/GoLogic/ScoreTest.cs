using GoLogic;
using GoLogic.Goban;
using GoLogic.Score;

namespace Tests.GoLogic
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

            (float black, float white) = gameScore.CalculateScore();

            Assert.Equal(6, black);
            Assert.Equal(9.5, white);
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

            (float black, float white) = gameScore.CalculateScore();
            
            Assert.Equal(2, black);
            Assert.Equal(6.5, white);
        }

        [Fact]
        public void ScoreRule_Chinese_OnEdge()
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
            gameLogic.SkipTurn();
            gameLogic.PlaceStone(0, 0);

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

            (float black, float white) = gameScore.CalculateScore();

            Assert.Equal(2, black);
            Assert.Equal(15.5, white);
        }

        [Fact]
        public void ScoreRule_DeadStoneRemoval_On2Liberties()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 2); // noir
            gameLogic.PlaceStone(0, 3); // blanc
            gameLogic.PlaceStone(1, 0); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(1, 1); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 1); // blanc
            gameLogic.SkipTurn(); // noir
            gameLogic.PlaceStone(2, 0); // blanc
            gameLogic.SkipTurn(); // noir
            gameLogic.PlaceStone(2, 3); // blanc

            // . . @ O . . . . .
            // @ @ @ O . . . . .
            // O O O O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . : vide, @ : noir, O : blanc

            Assert.True(gameScore.DeadStoneAnalyzer.IsGroupDead(gameBoard.GetStone(0, 2)));

        }

        //[Fact]
        //public void ScoreRule_DeadStoneRemoval_Isolated()
        //{
        //    GameBoard gameBoard = new GameBoard(9);
        //    GameLogic gameLogic = new GameLogic(gameBoard);
        //    ScoreRule gameScore = new ChineseScoreRule(gameBoard);

        //    gameLogic.PlaceStone(0, 0); // noir
        //    gameLogic.PlaceStone(0, 3); // blanc
        //    gameLogic.PlaceStone(0, 1); // noir
        //    gameLogic.PlaceStone(1, 3); // blanc
        //    gameLogic.SkipTurn(); // noir
        //    gameLogic.PlaceStone(2, 2); // blanc
        //    gameLogic.SkipTurn(); // noir
        //    gameLogic.PlaceStone(2, 1); // blanc
        //    gameLogic.SkipTurn(); // noir
        //    gameLogic.PlaceStone(2, 0); // blanc
        //    gameLogic.SkipTurn(); // noir
        //    gameLogic.PlaceStone(2, 3); // blanc

        //    // @ @ . O . . . . .
        //    // . . . O . . . . .
        //    // O O O O . . . . .
        //    // . . . . . . . . .
        //    // . . . . . . . . .
        //    // . . . . . . . . .
        //    // . . . . . . . . .
        //    // . . . . . . . . .
        //    // . . . . . . . . .
        //    // . : vide, @ : noir, O : blanc

        //    Assert.True(gameScore.IsGroupDead(gameBoard.GetStone(0, 0)));

        //}

        [Fact]
        public void ScoreRule_Ko_noDeadStoneRemoval()
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

            Assert.False(gameScore.DeadStoneAnalyzer.IsGroupDead(gameBoard.GetStone(2, 3)));
        }

        [Fact]
        public void ScoreRule_OneLiberty_noDeadStoneRemoval()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new JapaneseScoreRule(gameBoard);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(2, 1); // noir
            gameLogic.SkipTurn(); // blanc
            gameLogic.PlaceStone(3, 2); // noir
            

            // . . . . . . . . .
            // . . @ . . . . . .
            // . @ O . . . . . .
            // . . @ . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . : vide, @ : noir, O : blanc

            Assert.False(gameScore.DeadStoneAnalyzer.IsGroupDead(gameBoard.GetStone(2, 2)));
        }

        [Fact]
        public void ScoreRule_Seki_NoDeadStoneRemoval()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 4); // noir
            gameLogic.PlaceStone(0, 3); // blanc
            gameLogic.PlaceStone(0, 7); // noir
            gameLogic.PlaceStone(0, 6); // blanc
            gameLogic.PlaceStone(1, 4); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(2, 5); // noir
            gameLogic.PlaceStone(1, 6); // blanc
            gameLogic.PlaceStone(1, 7); // noir
            gameLogic.PlaceStone(2, 3); // blanc
            gameLogic.PlaceStone(2, 4); // noir
            gameLogic.PlaceStone(2, 6); // blanc
            gameLogic.PlaceStone(2, 7); // noir
            gameLogic.PlaceStone(3, 3); // blanc
            gameLogic.PlaceStone(3, 7); // noir
            gameLogic.PlaceStone(3, 4); // blanc
            gameLogic.PlaceStone(3, 6); // noir
            gameLogic.PlaceStone(3, 5); // blanc

            Assert.False(gameScore.DeadStoneAnalyzer.IsGroupDead(gameBoard.GetStone(0, 4)));
            Assert.False(gameScore.DeadStoneAnalyzer.IsGroupDead(gameBoard.GetStone(0, 6)));

            // . . . O @ . O @ .
            // . . . O @ . O @ .
            // . . . O @ @ O @ .
            // . . . O O O @ @ .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . : vide, @ : noir, O : blanc
        }

        [Fact]
        public void ScoreRule_TwoEyes_NoDeadStoneRemoval()
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

            Assert.False(gameScore.DeadStoneAnalyzer.IsGroupDead(gameBoard.GetStone(0, 4)));

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

            // . . O @ . @ O . .
            // . . O @ @ @ O . .
            // . . O O O O O . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // @ @ @ @ @ . . . .
            // . : vide, @ : noir, O : blanc

            gameScore.CalculateScore();

            Assert.Equal(StoneColor.White, gameBoard.GetStone(0, 2).Color);
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(1, 4).Color);
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(1, 3).Color);
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(8, 0).Color);
        }

        [Fact]
        public void Correct_Score_RemovedDeadStone_Chinese()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new ChineseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 2); // noir
            gameLogic.PlaceStone(0, 3); // blanc
            gameLogic.PlaceStone(1, 0); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(1, 1); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 1); // blanc
            gameLogic.PlaceStone(8, 0); // noir
            gameLogic.PlaceStone(2, 0); // blanc
            gameLogic.PlaceStone(8, 1); // noir
            gameLogic.PlaceStone(2, 3); // blanc

            // . . @ O . . . . .
            // @ @ @ O . . . . .
            // O O O O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // @ @ . . . . . . .
            // . : vide, @ : noir, O : blanc

            (float black, float white) = gameScore.CalculateScore();

            Assert.Equal(2, black);
            Assert.Equal(18.5, white);
        }

        [Fact]
        public void Correct_Score_RemovedDeadStone_Japanese()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new JapaneseScoreRule(gameBoard);

            gameLogic.PlaceStone(0, 2); // noir
            gameLogic.PlaceStone(0, 3); // blanc
            gameLogic.PlaceStone(1, 0); // noir
            gameLogic.PlaceStone(1, 3); // blanc
            gameLogic.PlaceStone(1, 1); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 1); // blanc
            gameLogic.PlaceStone(8, 0); // noir
            gameLogic.PlaceStone(2, 0); // blanc
            gameLogic.PlaceStone(8, 1); // noir
            gameLogic.PlaceStone(2, 3); // blanc

            // . . @ O . . . . .
            // @ @ @ O . . . . .
            // O O O O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // @ @ . . . . . . .
            // . : vide, @ : noir, O : blanc

            (float black, float white) = gameScore.CalculateScore();

            Assert.Equal(0, black);
            Assert.Equal(12.5, white);
        }

        [Fact] //TODO: !!! Penser à le retier
        public void No361bug()
        {
            GameBoard gameBoard = new GameBoard(9);
            GameLogic gameLogic = new GameLogic(gameBoard);
            ScoreRule gameScore = new JapaneseScoreRule(gameBoard);

            gameLogic.PlaceStone(8, 8);
            (float black, float white) = gameScore.CalculateScore();

            Assert.Equal(0, black);
            Assert.Equal(6.5, white);
        }
    }
}
