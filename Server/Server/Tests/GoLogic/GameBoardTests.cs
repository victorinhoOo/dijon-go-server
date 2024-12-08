using GoLogic;
using GoLogic.Goban;

namespace Tests.GoLogic
{
    public class GameBoardTests
    {
        [Fact]
        public void GameBoard_Initialization_EmptyStones()
        {
            // Organise
            int size = 9;
            var gameBoard = new GameBoard(size);

            // Fait & Assert
            Assert.Equal(size, gameBoard.Size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Assert.Equal(StoneColor.Empty, gameBoard.GetStone(i, j).Color);
                }
            }
        }

        [Fact]
        public void GetStone_ValidCoordinates_ReturnsStone()
        {
            // Organise
            var gameBoard = new GameBoard(9);

            // Fait
            var stone = gameBoard.GetStone(0, 0);

            // Assert
            Assert.NotNull(stone);
            Assert.Equal(0, stone.X);
            Assert.Equal(0, stone.Y);
        }

        [Fact]
        public void GetStone_InvalidCoordinates_ThrowsException()
        {
            // Organise
            var gameBoard = new GameBoard(9);

            // Fait & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => gameBoard.GetStone(10, 10));
        }

        [Fact]
        public void IsValidCoordinate_WithinBounds_ReturnsTrue()
        {
            // Organise
            var gameBoard = new GameBoard(9);

            // Fait & Assert
            Assert.True(gameBoard.IsValidCoordinate(0, 0));
            Assert.False(gameBoard.IsValidCoordinate(9, 9)); // Outside of bounds
        }

        [Fact]
        public void CopieBoard_copieCorrectly()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);
            gameLogic.PlaceStone(0, 0);
            gameLogic.PlaceStone(1, 1);

            IBoard gameBoard2  = gameBoard.Clone();

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(0, 0).Color);
            Assert.Equal(StoneColor.White, gameBoard.GetStone(1, 1).Color);
        }

        [Fact]
        public void CopyBoard_NotAffected()
        {
            var gameBoard = new GameBoard(9);
            var gameLogic = new GameLogic(gameBoard);

            IBoard gameboard2 = gameBoard.Clone();

            gameLogic.PlaceStone(0, 0); // noir
            gameLogic.PlaceStone(1, 1); // blanc

            Assert.NotEqual(StoneColor.Black, gameboard2.GetStone(0, 0).Color);
            Assert.NotEqual(StoneColor.White, gameboard2.GetStone(1, 1).Color);

        }

        [Fact]
        public void HandicapOf6_On19by19()
        {
            var gameBoard = new GameBoard(19, "black", 6);

            Assert.Equal(StoneColor.Black, gameBoard.GetStone( 3, 15).Color); // A
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(15, 3).Color); // B
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(15, 15).Color); // C
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 3).Color); // D
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone( 9, 9 ).Color); // E empty
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 3).Color); // F
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 15).Color); // G
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone( 3, 9 ).Color); // H empty
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(15, 9 ).Color); // I empty
        }

        [Fact]
        public void HandicapOf8_On19by19()
        {
            var gameBoard = new GameBoard(19, "black", 8);

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 15).Color); // A
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(15, 3).Color); // B
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(15, 15).Color); // C
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 3).Color); // D
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone( 9, 9 ).Color); // E empty
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 3).Color); // F
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 15).Color); // G
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 9).Color); // H
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(15, 9).Color); // I
        }

        [Fact]
        public void HandicapOf6_On13by13()
        {
            var gameBoard = new GameBoard(13, "black", 6);

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 9).Color); // A
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 3).Color); // B
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 9).Color); // C
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 3).Color); // D
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(6, 6).Color); // E
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(6, 3).Color); // F
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(6, 9).Color); // G
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(9, 6).Color); // I
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(3, 6).Color); // H
        }

        [Fact]
        public void HandicapOf8_On13by13()
        {
            var gameBoard = new GameBoard(13, "black", 8);

            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 9).Color); // A
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 3).Color); // B
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 9).Color); // C
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 3).Color); // D
            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(6, 6).Color); // E
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(6, 3).Color); // F
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(6, 9).Color); // G
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(3, 6).Color); // H
            Assert.Equal(StoneColor.Black, gameBoard.GetStone(9, 6).Color); // I
        }

        [Fact]
        public void Handicap_On_19by19()
        {
            List<(int x, int y)> hoshis = [(3, 15), (15, 3), (15, 15), (3, 3), (9, 9), (9, 3), (9, 15), (3, 9), (15, 9)];

            for (int i = 0; i <= hoshis.Count; i++)
            {
                List<(int x, int y)> hoshiSlice = hoshis.Slice(0, i);
                var gameBoard = new GameBoard(19, "white", hoshiSlice.Count);

                if (i != 6 && i != 8)
                {
                    foreach ((int x, int y) in hoshiSlice)
                    {
                        Assert.Equal(StoneColor.White, gameBoard.GetStone(x, y).Color);
                    }

                    // Parcourir chaque case du board
                    for (int x = 0; x < gameBoard.Size; x++)
                    {
                        for (int y = 0; y < gameBoard.Size; y++)
                        {
                            // Vérifier si la case actuelle ne fait pas partie des coordonnées fournies
                            if (!hoshiSlice.Contains((x, y)))
                            {
                                Assert.Equal(StoneColor.Empty, gameBoard.GetStone(x, y).Color);
                            }
                        }
                    }
                }

                
            }
        }

        [Fact]
        public void Handicap_On_13by13()
        {
            List<(int x, int y)> hoshis = [(3, 9), (9, 3), (9, 9), (3, 3), (6, 6), (6, 3), (6, 9), (3, 6), (9, 6)];

            for (int i = 6; i <= hoshis.Count; i++)
            {
                List<(int x, int y)> hoshiSlice = hoshis.Slice(0, i);
                var gameBoard = new GameBoard(13, "black", hoshiSlice.Count);

                if (i != 6 && i != 8)
                {
                    foreach ((int x, int y) in hoshiSlice)
                    {
                        Assert.Equal(StoneColor.Black, gameBoard.GetStone(x, y).Color);
                    }

                    // Parcourir chaque case du board
                    for (int x = 0; x < gameBoard.Size; x++)
                    {
                        for (int y = 0; y < gameBoard.Size; y++)
                        {
                            // Vérifier si la case actuelle ne fait pas partie des coordonnées fournies
                            if (!hoshiSlice.Contains((x, y)))
                            {
                                Assert.Equal(StoneColor.Empty, gameBoard.GetStone(x, y).Color);
                            }
                        }
                    }
                }
                
            }
        }

        [Fact]
        public void Handicap_On_9by9()
        {
            List<(int x, int y)> hoshis = [(2, 6), (6, 2), (6, 6), (2, 2)];

            for (int i = 0; i <= hoshis.Count; i++)
            {
                List<(int x, int y)> hoshiSlice = hoshis.Slice(0, i);
                var gameBoard = new GameBoard(9, "white", hoshiSlice.Count);

                foreach ((int x, int y) in hoshiSlice)
                {
                    Assert.Equal(StoneColor.White, gameBoard.GetStone(x, y).Color);
                }

                // Parcourir chaque case du board
                for (int x = 0; x < gameBoard.Size; x++)
                {
                    for (int y = 0; y < gameBoard.Size; y++)
                    {
                        // Vérifier si la case actuelle ne fait pas partie des coordonnées fournies
                        if (!hoshiSlice.Contains((x, y)))
                        {
                            Assert.Equal(StoneColor.Empty, gameBoard.GetStone(x, y).Color);
                        }
                    }
                }
            }
        }
    }
}
