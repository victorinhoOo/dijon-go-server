using Xunit;
using WebSocket.Strategy;
using WebSocket.Strategy.Enumerations;
using WebSocket.Model;
using Tests.WebSockets.FakeDAO;
using WebSocket.Model.DAO;
using System.Globalization;
using WebSocket;
using GoLogic.Goban;
using GoLogic.Score;
using GoLogic.Serializer;
using GoLogic.Timer;
using GoLogic;
using System.Collections.Concurrent;
using Server.Model.Managers;

namespace Tests.WebSocketTest
{
    public class StrategyTests
    {
        private MockClient mockPlayer1;
        private MockClient mockPlayer2;
        private FakeGameDAO fakeGameDAO;
        private FakeUserDAO fakeUserDAO;
        private Game testGame;

        public void StrategyTest()
        {
            // Setup mock clients
            this.mockPlayer1 = new MockClient();
            mockPlayer2 = new MockClient();

            // Setup fake DAOs
            fakeGameDAO = new FakeGameDAO();
            fakeUserDAO = new FakeUserDAO();

            // Create a test game
            var config = new GameConfiguration(13, "c", 7.5f, "custom-game", 6, "black");

            var gameBoard = new GameBoard(config.Size);
            var logic = new GameLogic(gameBoard);
            var boardSerializer = new BoardSerializer(logic);
            var scoreRule = ScoreRuleFactory.Create(config.Rule, gameBoard, config.Komi);
            var gameManager = new WebSocket.Model.Managers.GameManager(fakeUserDAO, fakeGameDAO);
            var timerManager = new TimerManager();

            testGame = new Game(config, gameBoard, logic, boardSerializer, scoreRule, gameManager, timerManager);
            testGame.Player1 = mockPlayer1;
            testGame.Player2 = mockPlayer2;
            testGame.ChangeTurn();

            // Initialise Server static dictionnaire
            WebSocket.Server.CustomGames = new ConcurrentDictionary<int, Game>();
            WebSocket.Server.MatchmakingGames = new ConcurrentDictionary<int, Game>();

            // Ajoute le Game au dicitionnaire de Server
            WebSocket.Server.CustomGames.TryAdd(testGame.Id, testGame);
            WebSocket.Server.ConnectedClients.TryAdd(fakeUserDAO.GetUserByToken("token1").Name, testGame.Player1);
            WebSocket.Server.ConnectedClients.TryAdd(fakeUserDAO.GetUserByToken("token2").Name, testGame.Player2);
        }

        [Fact]
        public void PlaceStoneStrategy_Execute_ValidMove()
        {
            StrategyTest();
            StrategyTest();
            // Arrange
            var strategy = new PlaceStoneStrategy();
            string[] data = new[] { "2", "Stone", "2", "3" };
            GameType gameType = GameType.CUSTOM;
            string response = "";
            string type = "";

            // Act
            strategy.Execute(mockPlayer1, data, gameType, ref response, ref type);

            // Assert
            Assert.Equal("Broadcast_", type);
            Assert.Contains("Stone", response);
        }
        
        [Fact]
        public void CreateGameStrategy_Execute_CreatesGame()
        {
            StrategyTest();
            // Arrange
            var availableGameManager = new MockAvailableGameManager(fakeGameDAO);
            var strategy = new CreateGameStrategyWithFake(availableGameManager, new WebSocket.Model.Managers.GameManager(fakeUserDAO, fakeGameDAO));
            string[] data = new[] { "msg", "Create", "", "19", "c", "6.5", "TestGame", "0", "black" };
            GameType gameType = GameType.CUSTOM;
            string response = "";
            string type = "";

            // Act
            strategy.Execute(mockPlayer1, data, gameType, ref response, ref type);

            // Assert
            Assert.Equal("Send_", type);
            Assert.Contains("2-", response);
        }

        [Fact]
        public void SkipStrategy_Execute_SkipsTurn()
        {
            StrategyTest();
            // Arrange
            var strategy = new SkipStrategy();
            string[] data = new[] { "2", "Skip" };
            GameType gameType = GameType.CUSTOM;
            string response = "";
            string type = "";

            // Act
            strategy.Execute(mockPlayer1, data, gameType, ref response, ref type);

            // Assert
            Assert.Equal("Broadcast_", type);
            Assert.Contains("Skipped", response.Split("-")[1]);
        }

        [Fact]
        public void CancelStrategy_Execute_CancelsGame()
        {
            StrategyTest();
            // Arrange
            var strategy = new CancelStrategy();
            string[] data = new[] { "1", "Cancel" };
            GameType gameType = GameType.MATCHMAKING;
            string response = "";
            string type = "";

            // Act
            strategy.Execute(mockPlayer1, data, gameType, ref response, ref type);

            // Assert
            Assert.Equal("Broadcast_", type);
            Assert.Contains("Cancelled", response.Split("-")[1]);
        }

        public class MockAvailableGameManager
        {
            private IGameDAO gameDAO;
            public MockAvailableGameManager(IGameDAO gameDao)
            {
                this.gameDAO = gameDao;
            }

            /// <summary>
            /// Insère une partie disponible en base de données (appelée lors de la création d'une partie personnalisée)
            /// </summary>
            /// <param name="game">La partie à insérer</param>
            public void InsertAvailableGame(Game game)
            {
                gameDAO.InsertAvailableGame(game);
            }

            /// <summary>
            /// Supprime une partie disponible en base de données (appelee lors de la suppression d'une partie personnalisée)
            /// </summary>
            /// <param name="id">La partie à supprimer</param>
            public void DeleteAvailableGame(int id)
            {
                gameDAO.DeleteAvailableGame(id);
            }
        }

        public class CreateGameStrategyWithFake : IStrategy
        {

            // Constantes pour les index de tableau
            private const int SIZE_INDEX = 3;
            private const int RULE_INDEX = 4;
            private const int KOMI_INDEX = 5;
            private const int NAME_INDEX = 6;
            private const int HANDICAP_INDEX = 7;
            private const int COLOR_HANDICAP_INDEX = 8;

            private MockAvailableGameManager availableGameManager;
            private WebSocket.Model.Managers.GameManager gameManager;

            public CreateGameStrategyWithFake(MockAvailableGameManager availableGameManager, WebSocket.Model.Managers.GameManager gameManager)
            {
                this.availableGameManager = availableGameManager;
            }

            /// <summary>
            /// Exécute la création d'une nouvelle partie
            /// </summary>
            /// <param name="player">Le joueur qui créé la partie</param>
            /// <param name="data">Les données du message sous forme de tableau de chaînes</param>
            /// <param name="gameType">Le type de partie concernée ("custom" ou "matchmaking")</param>
            /// <param name="response">La réponse à envoyer au client (modifiée par référence)</param>
            /// <param name="type">Le type de réponse (modifié par référence)</param>
            public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type)
            {
                if (gameType == GameType.CUSTOM) // la partie est personnalisée
                {
                    int size = Convert.ToInt16(data[SIZE_INDEX]);
                    string rule = data[RULE_INDEX];
                    string name = data[NAME_INDEX];
                    float komi = float.Parse(data[KOMI_INDEX], CultureInfo.InvariantCulture.NumberFormat);
                    int handicap = int.Parse(data[HANDICAP_INDEX]);
                    string colorHandicap = data[COLOR_HANDICAP_INDEX];
                    int id = WebSocket.Server.CustomGames.Count + 1;
                    GameConfiguration config = new GameConfiguration(size, rule, komi, name, handicap, colorHandicap);
                    Game newGame = CreateCustomGameWithFakes(config, this.gameManager);
                    newGame.AddPlayer(player);
                    WebSocket.Server.CustomGames[id] = newGame;
                    availableGameManager.InsertAvailableGame(newGame); // Ajout de la partie dans le dictionnaire des parties
                    WebSocket.Server.CustomGames[id].Player1 = player; // Ajout du client en tant que joueur 1
                    response = $"{id}-"; // Renvoi de l'id de la partie créée
                    type = "Send_";
                }
                else if (gameType == GameType.MATCHMAKING)
                {
                    int id = WebSocket.Server.MatchmakingGames.Count + 1;
                    Game newGame = GameFactory.CreateMatchmakingGame();
                    newGame.AddPlayer(player);
                    WebSocket.Server.MatchmakingGames[id] = newGame;
                    WebSocket.Server.MatchmakingGames[id].Player1 = player;
                    response = $"{id}-"; // Renvoi del'id de la partie créée
                    type = "Send_";
                }
            }
        }

        private static Game CreateCustomGameWithFakes(GameConfiguration config, WebSocket.Model.Managers.GameManager gameManager)
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
