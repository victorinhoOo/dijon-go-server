using GoLogic;
using GoLogic.Goban;

namespace Tests.GoLogic
{
    public class StoneTests
    {
        [Fact]
        public void Stone_Initialization_CorrectAttributes()
        {
            // Organise & Fait
            var stone = new Stone(1, 1, StoneColor.Black);

            // Assert
            Assert.Equal(1, stone.X);
            Assert.Equal(1, stone.Y);
            Assert.Equal(StoneColor.Black, stone.Color);
        }

        [Fact]
        public void Stone_Equals_TrueForSameAttributes()
        {
            // Organise
            var stone1 = new Stone(1, 1, StoneColor.Black);
            var stone2 = new Stone(1, 1, StoneColor.Black);

            // Fait & Assert
            Assert.True(stone1.Equals(stone2));
        }

        [Fact]
        public void Stone_Equals_FalseForDifferentAttriubtes()
        {
            // Organise
            var stone1 = new Stone(1, 1, StoneColor.White);
            var stone2 = new Stone(1, 1, StoneColor.Black);

            // Fait & Assert
            Assert.False(stone1.Equals(stone2));
        }

        [Fact]
        public void GetNeighborsCoordinate_ReturnsCorrectCoordinates()
        {
            // Organise
            var stone = new Stone(2, 2);

            // Fait
            var neighbors = stone.GetNeighborsCoordinate();

            // Assert
            Assert.Contains((3, 2), neighbors);
            Assert.Contains((1, 2), neighbors);
            Assert.Contains((2, 3), neighbors);
            Assert.Contains((2, 1), neighbors);
        }
    }
}
