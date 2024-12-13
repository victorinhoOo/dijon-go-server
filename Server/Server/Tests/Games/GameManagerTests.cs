using Xunit;
using Moq;
using Server.Model.Managers;
using Server.Model.Data;
using Server.Model.DTO;

namespace Tests.Games
{
    /// <summary>
    /// Classe de tests pour la récupération des parties
    /// </summary>
    public class GameManagerTests
    {
        private GameManager gameManager;
        private FakeGameDAO fakeGameDAO;

        public GameManagerTests()
        {
            fakeGameDAO = new FakeGameDAO();
            gameManager = new GameManager(fakeGameDAO);
        }

        [Fact]
        public void GetAvailableGames_ReturnsAvailableGames()
        {
            // Act
            var result = gameManager.GetAvailableGames();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Game1", result[0].Name);
            Assert.Equal("Game2", result[1].Name);
        }

        [Fact]
        public void GetGamesByToken_ReturnsGames()
        {
            // Act
            var result = gameManager.GetGamesByToken("someToken");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Alice", result[0].UsernamePlayer1);
            Assert.Equal("Bob", result[0].UsernamePlayer2);
        }

        [Fact]
        public void GetGameStatesByGameId_ReturnsGameStates()
        {
            // Act
            var result = gameManager.GetGameStatesByGameId(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("BoardState1", result[0].Board);
            Assert.Equal(0, result[0].CapturedBlack);
        }
    }
}