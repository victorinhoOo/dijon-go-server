using GoLogic.Goban;
using GoLogic.Score;
using GoLogic.Serializer;
using GoLogic.Timer;
using GoLogic;
using Tests.WebSockets.FakeDAO;
using WebSocket.Model;
using WebSocket.Model.Managers;

namespace Tests.WebSocketTest
{
    public class GameFactoryTests
    {

        private readonly FakeUserDAO fakeUserDAO;
        private readonly FakeGameDAO fakeGameDAO;

        public GameFactoryTests()
        {
            fakeUserDAO = new FakeUserDAO();
            fakeGameDAO = new FakeGameDAO();
        }

        [Fact]
        public void CreateCustomGame_WithValidConfig_ReturnsCorrectGame()
        {
            // Arrange
            var config = new GameConfiguration(13, "c", 7.5f, "custom-game", 6, "black");

            var gameManager = new GameManager(fakeUserDAO, fakeGameDAO);

            // Act
            Game game = CreateCustomGameWithFakes(config, gameManager);

            // Assert
            Assert.NotNull(game);
            Assert.Equal(13, game.Config.Size);
            Assert.Equal(6, game.Config.Handicap);
            Assert.Equal("c", game.Config.Rule);
            Assert.Equal(7.5f, game.Config.Komi);
            Assert.Equal("custom-game", game.Config.Name);
        }

        [Fact]
        public void CreateMatchmakingGame_ReturnsGameWithDefaultConfiguration()
        {
            // Act
            var gameManager = new GameManager(fakeUserDAO, fakeGameDAO);
            var defaultConfig = GameConfiguration.Default();

            var game = CreateCustomGameWithFakes(defaultConfig, gameManager);

            // Assert
            Assert.NotNull(game);
            Assert.Equal(19, game.Config.Size);
            Assert.Equal(0, game.Config.Handicap);
            Assert.Equal("j", game.Config.Rule);
            Assert.Equal(6.5f, game.Config.Komi);
            Assert.Equal("matchmaking-game", game.Config.Name);

        }

        private Game CreateCustomGameWithFakes(GameConfiguration config, GameManager gameManager)
        {
            var gameBoard = new GameBoard(config.Size, config.HandicapColor, config.Handicap);
            var logic = new GameLogic(gameBoard);
            var boardSerializer = new BoardSerializer(logic);
            var scoreRule = ScoreRuleFactory.Create(config.Rule, gameBoard, config.Komi);
            var timerManager = new TimerManager();

            return new Game(config, gameBoard, logic, boardSerializer, scoreRule, gameManager, timerManager);
        }
    }
}
