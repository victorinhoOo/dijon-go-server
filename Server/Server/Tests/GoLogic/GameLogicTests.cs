using GoLogic;
using GoLogic.Goban;
using GoLogic.Score;

namespace Tests.GoLogic
{
    public class GameLogicTests
    {
        [Fact]
        public void Create_various_Board_Size()
        {
            // Organise
            int neuf = 9;
            int treize = 13;
            int dixneuf = 19;

            var gameBoard9 = new GameBoard(neuf);
            var gameBoard13 = new GameBoard(treize);
            var gameBoard19 = new GameBoard(dixneuf);
            var gameLogic9 = new GameLogic(gameBoard9);
            var gameLogic13 = new GameLogic(gameBoard13);
            var gameLogic19 = new GameLogic(gameBoard19);

            // fait
            gameLogic9.PlaceStone(0, 0);
            gameLogic13.PlaceStone(0, 0);
            gameLogic19.PlaceStone(0, 0);

            gameLogic9.PlaceStone(neuf - 1, neuf - 1);
            gameLogic13.PlaceStone(treize - 1, treize - 1);
            gameLogic19.PlaceStone(dixneuf - 1, dixneuf - 1);

            // Assert
            Assert.Equal(neuf, gameBoard9.Size);
            Assert.Equal(treize, gameBoard13.Size);
            Assert.Equal(dixneuf, gameBoard19.Size);

            Assert.Equal(neuf, gameBoard9.Size);
            Assert.Equal(treize, gameBoard13.Size);
            Assert.Equal(dixneuf, gameBoard19.Size);

            Assert.NotEqual(StoneColor.Empty, gameBoard9.GetStone(0, 0).Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard13.GetStone(0, 0).Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard19.GetStone(0, 0).Color);

            Assert.NotEqual(StoneColor.Empty, gameBoard9.GetStone(neuf -1, neuf -1).Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard13.GetStone(treize - 1, treize - 1).Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard19.GetStone(dixneuf - 1, dixneuf - 1).Color);
        }

        [Fact]
        public void Test_skipTurn()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            // Fait
            gameLogic.PlaceStone(1, 1); // pierre noire
            gameLogic.SkipTurn(); // blanc saute son tour
            gameLogic.PlaceStone(2, 2); // devrait aussi être noire
            gameLogic.SkipTurn(); // blanc saute son tour
            gameLogic.PlaceStone(0, 0); // devrait aussi petre noire

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(0, 0).Color);
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(1, 1).Color);
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(2, 2).Color);
        }

        [Fact]
        public void EngGame()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            // Fait
            gameLogic.PlaceStone(1, 1); // pierre noire
            gameLogic.PlaceStone(0, 0); // pierre blanche
            gameLogic.SkipTurn(); // noir saute son tour
            gameLogic.PlaceStone(2, 2); // devrait être blanc
            gameLogic.SkipTurn(); // noir saute son tour
            gameLogic.SkipTurn(); // blanc saute son tour

            // Assert
            Assert.Equal(StoneColor.White, gameBoard.GetStone(2, 2).Color);
            Assert.True(gameLogic.IsEndGame);
        }

        [Fact]
        public void PlaceStone_ValidMove_ReturnsTrue()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            // Fait
            bool isValid = gameLogic.PlaceStone(1, 1);

            // Assert
            Assert.True(isValid);
            Assert.Equal(1, gameBoard.GetStone(1, 1).X);
            Assert.Equal(1, gameBoard.GetStone(1, 1).Y);
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(1, 1).Color); // Black stone placed
        }

        [Fact]
        public void SkipTurn_doSkipe()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            gameLogic.SkipTurn();

            Assert.Equal(StoneColor.White, gameLogic.CurrentTurn);
        }

        [Fact]
        public void PlaceStone_InvalidOperation_ThrowsException()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);
            gameLogic.PlaceStone(1, 1);

            // Fait & Assert
            Assert.Throws<InvalidOperationException>(() => gameLogic.PlaceStone(1, 1));
        }

        [Fact]
        public void ScoreRule_SekiSituation()
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

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(0, 4).Color);
            Assert.Equal(StoneColor.White, gameBoard.GetStone(0, 6).Color);

            gameLogic.PlaceStone(0, 5); // noir

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(0, 4).Color);
            Assert.Equal(StoneColor.White, gameBoard.GetStone(0, 6).Color);

            gameLogic.PlaceStone(1, 5); // blanc capture noir

            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(0, 4).Color);
            Assert.Equal(StoneColor.White, gameBoard.GetStone(0, 6).Color);
        }

        [Fact]
        public void IsKoViolation_NoViolation_ReturnsFalse()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            // Fait
            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(2, 2); // blanc
            bool isKo = gameBoard.IsKoViolation();

            // Assert
            Assert.False(isKo); // Initially, no Ko violation
        }

        [Fact]
        public void Ko_Rule_work()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

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
            // . @ O . O . . . .
            // . . @ O . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . @ . . . . .

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

            // Fait & Assert
            Assert.Throws<InvalidOperationException>(() => gameLogic.PlaceStone(2, 2)); // bc refusé par la régle de ko
        }

        [Fact]
        public void CapturesOpponent_One_FullySurounded()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

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

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(2, 3).Color);
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(2, 2).Color);
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

        [Fact]
        public void CaptureOpponent_Group_onEdge()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            gameLogic.PlaceStone(1, 2); // noir
            gameLogic.PlaceStone(0, 0); // blanc
            gameLogic.PlaceStone(1, 0); // noir
            gameLogic.PlaceStone(0, 1); // blanc
            gameLogic.PlaceStone(1, 1); // noir
            gameLogic.PlaceStone(0, 2); // blanc

            // # : vide, + : noir, - : blanc
            // - - - # # # # # #
            // + + + # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #

            gameLogic.PlaceStone(0, 3); // noir capture blanc en (0, 0) et (0, 1)

            // # : vide, + : noir, - : blanc
            // # # # + # # # # #
            // + + + # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #
            // # # # # # # # # #

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(0, 3).Color);
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(0, 0).Color);
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(0, 1).Color);
        }

        [Fact]
        public void GetNeighbors_Returns_neighbors()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            List<Stone> neighbor00 = [];
            List<Stone> neighbor11 = [];
            
            gameLogic.PlaceStone(0, 0); // noir
            gameLogic.PlaceStone(1, 1); // blanc
            gameLogic.PlaceStone(1, 0); // noir
            gameLogic.PlaceStone(0, 1); // blanc
            gameLogic.PlaceStone(1, 2); // noir


            // . : vide, @ : noir, O : blanc
            // @ O . . . . . . .
            // @ O @ . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .
            // . . . . . . . . .

            neighbor11.Add(gameBoard.GetStone(2, 1));
            neighbor11.Add(gameBoard.GetStone(0, 1));
            neighbor11.Add(gameBoard.GetStone(1, 2));
            neighbor11.Add(gameBoard.GetStone(1, 0));

            neighbor00.Add(gameBoard.GetStone(1, 0));
            neighbor00.Add(gameBoard.GetStone(0, 1));
            
            Assert.Equal(neighbor11, gameBoard.GetNeighbors(gameBoard.GetStone(1, 1)));
            Assert.Equal(neighbor00, gameBoard.GetNeighbors(gameBoard.GetStone(0, 0)));
        }
    }
}
