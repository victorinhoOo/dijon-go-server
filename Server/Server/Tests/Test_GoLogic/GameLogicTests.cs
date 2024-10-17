using GoLogic;
using Org.BouncyCastle.Asn1.BC;

namespace Test_GoLogic
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
            Assert.Equal(neuf, gameBoard9.Board.GetLength(0));
            Assert.Equal(treize, gameBoard13.Board.GetLength(0));
            Assert.Equal(dixneuf, gameBoard19.Board.GetLength(0));

            Assert.Equal(neuf, gameBoard9.Board.GetLength(1));
            Assert.Equal(treize, gameBoard13.Board.GetLength(1));
            Assert.Equal(dixneuf, gameBoard19.Board.GetLength(1));

            Assert.Equal(neuf, gameBoard9.Size);
            Assert.Equal(treize, gameBoard13.Size);
            Assert.Equal(dixneuf, gameBoard19.Size);

            Assert.NotEqual(StoneColor.Empty, gameBoard9.Board[0, 0].Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard13.Board[0, 0].Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard19.Board[0, 0].Color);

            Assert.NotEqual(StoneColor.Empty, gameBoard9.Board[neuf -1, neuf -1].Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard13.Board[treize - 1, treize -1].Color);
            Assert.NotEqual(StoneColor.Empty, gameBoard19.Board[dixneuf - 1, dixneuf - 1].Color);
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

            Assert.Equal(StoneColor.Black, gameBoard.Board[0, 0].Color);
            Assert.Equal(StoneColor.Black, gameBoard.Board[1, 1].Color);
            Assert.Equal(StoneColor.Black, gameBoard.Board[2, 2].Color);
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
            Assert.Equal(StoneColor.White, gameBoard.Board[2, 2].Color);
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
            Assert.Equal(1, gameBoard.Board[1, 1].X);
            Assert.Equal(1, gameBoard.Board[1, 1].Y);
            Assert.Equal(StoneColor.Black, gameBoard.Board[1, 1].Color); // Black stone placed
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
        public void IsKoViolation_NoViolation_ReturnsFalse()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            // Fait
            gameBoard.Board[1,0].Color = StoneColor.Black;
            bool isKo = gameLogic.IsKoViolation();

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

            Assert.Equal(StoneColor.Black, gameBoard.Board[2, 3].Color);
            Assert.Equal(StoneColor.Empty, gameBoard.Board[2, 2].Color);
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

            Assert.Equal(StoneColor.Black, gameBoard.Board[0, 3].Color);
            Assert.Equal(StoneColor.Empty, gameBoard.Board[0, 0].Color);
            Assert.Equal(StoneColor.Empty, gameBoard.Board[0, 1].Color);
        }

        //[Fact]
        //public void GetNeighbors_Returns_neighbors()
        //{
        //    var gameBoard = new GameBoard(9);
        //    var gameLogic = new GameLogic(gameBoard);

        //    gameLogic.PlaceStone(1, 2); // noir
        //    gameLogic.PlaceStone(2, 2); // blanc
        //    gameLogic.PlaceStone(2, 1); // noir
        //    gameLogic.PlaceStone(1, 3); // blanc
        //    gameLogic.PlaceStone(3, 2); // noir

        //    List<Stone> neighbor = [];
        //    neighbor.Add(gameBoard.GetStone(2, 2));
        //    neighbor.Add(gameBoard.GetStone(1, 3));

        //    Assert.Equal(neighbor, gameLogic.GetNeighbors(gameBoard.GetStone(1, 2));
        //}
    }
}
