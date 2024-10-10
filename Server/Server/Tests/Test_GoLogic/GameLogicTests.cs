using Go_logic_csharp;

namespace Test_GoLogic
{
    public class GameLogicTests
    {
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
        public void PlaceStone_InvalidMove_ReturnsFalse()
        {
            // Organise
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);
            gameLogic.PlaceStone(1, 1); // First move

            // Fait
            bool isValid = gameLogic.PlaceStone(1, 1); // Invalid move, same position

            // Assert
            Assert.False(isValid);
        }

        //[Fact]
        //public void PlaceStone_InvalidOperation_ThrowsException()
        //{
        //    // Organise
        //    var gameBoard = new GameBoard(9);
        //    var gameLogic = new GameLogic(gameBoard);
        //    gameLogic.PlaceStone(1, 1);

        //    // Fait & Assert
        //    Assert.Throws<InvalidOperationException>(() => gameLogic.PlaceStone(1, 1));
        //}

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
