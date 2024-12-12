using Xunit;
using WebSocket.Model;
using WebSocket.Model.Managers;
using Tests.WebSockets.FakeDAO;

namespace Tests.WebSocketTest
{   
    public class GameManagerTests
    {
        private GameManager gameManager;
        private FakeUserDAO fakeUserDAO;
        private FakeGameDAO fakeGameDAO;
        private MockClient mockClient1;
        private MockClient mockClient2;

        public GameManagerTests()
        {
            fakeUserDAO = new FakeUserDAO();
            fakeGameDAO = new FakeGameDAO();
            gameManager = new GameManager(fakeUserDAO, fakeGameDAO);
            mockClient1 = new MockClient();
            mockClient2 = new MockClient();
        }

        [Fact]
        public void GetUserByToken_ExistingToken_ReturnsCorrectUser()
        {
            // Arrange
            string token = "token1";

            // Act
            var result = gameManager.GetUserByToken(token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
            Assert.Equal(2000, result.Elo);
        }

        [Fact]
        public void GetUserByToken_NonExistingToken_ReturnsNull()
        {
            // Arrange
            string token = "nonexistenttoken";

            // Act
            var result = gameManager.GetUserByToken(token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateEloWinnerLooser_UpdatesEloCorrectly()
        {
            // Arrange
            var winner = gameManager.GetUserByToken("token1");
            var looser = gameManager.GetUserByToken("token2");
            int initialWinnerElo = winner.Elo;
            int initialLooserElo = looser.Elo;

            // Act
            gameManager.UpdateEloWinnerLooser(winner, looser);

            // Assert
            Assert.True(winner.Elo > initialWinnerElo, "Winner's Elo should increase");
            Assert.True(looser.Elo < initialLooserElo, "Looser's Elo should decrease");
        }

        [Fact]
        public void UpdateEloWinnerLooser_WithEqualElo_MakesExpectedChanges()
        {
            // Arrange
            var winner = gameManager.GetUserByToken("token4");
            var looser = gameManager.GetUserByToken("token5");
            int initialElo = winner.Elo;

            // Act
            gameManager.UpdateEloWinnerLooser(winner, looser);

            // Assert
            Assert.Equal(116, winner.Elo); 
            Assert.Equal(84, looser.Elo); 
        }
    }
}
