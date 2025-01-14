using WebSocket.Model;
using WebSocket.Strategy.Enumerations;
using WebSocket.Model.Managers;
using System.Globalization;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie pour la création d'une nouvelle partie de jeu
    /// </summary>
    public class CreateGameStrategy : IStrategy
    {
        // Constantes pour les index de tableau
        private const int SIZE_INDEX = 3;
        private const int RULE_INDEX = 4;
        private const int KOMI_INDEX = 5;
        private const int NAME_INDEX = 6;
        private const int HANDICAP_INDEX = 7;
        private const int COLOR_HANDICAP_INDEX = 8;

        private readonly AvailableGameManager availableGameManager;

        public CreateGameStrategy()
        {
            this.availableGameManager = new AvailableGameManager();
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
            int id = GenerateGameId(gameType);
            Game newGame = CreateGame(player, data, gameType);
            
            StoreGame(newGame, id, gameType);
            SetResponse(id, ref response, ref type);
        }

        /// <summary>
        /// Génère un nouvel ID de partie
        /// </summary>
        private int GenerateGameId(GameType gameType)
        {
            return gameType == GameType.CUSTOM 
                ? Server.CustomGames.Count + 1 
                : Server.MatchmakingGames.Count + 1;
        }

        /// <summary>
        /// Crée une nouvelle partie selon le type spécifié
        /// </summary>
        private Game CreateGame(IClient player, string[] data, GameType gameType)
        {
            Game newGame = gameType == GameType.CUSTOM 
                ? CreateCustomGame(data)
                : CreateMatchmakingGame();

            newGame.AddPlayer(player);
            newGame.Player1 = player;
            
            return newGame;
        }

        /// <summary>
        /// Crée une partie personnalisée avec les paramètres spécifiés
        /// </summary>
        private Game CreateCustomGame(string[] data)
        {
            GameConfiguration config = CreateGameConfiguration(data);
            return GameFactory.CreateCustomGame(config);
        }

        /// <summary>
        /// Crée une configuration de jeu à partir des données
        /// </summary>
        private GameConfiguration CreateGameConfiguration(string[] data)
        {
            return new GameConfiguration(
                size: Convert.ToInt16(data[SIZE_INDEX]),
                rule: data[RULE_INDEX],
                komi: float.Parse(data[KOMI_INDEX], CultureInfo.InvariantCulture.NumberFormat),
                name: data[NAME_INDEX],
                handicap: int.Parse(data[HANDICAP_INDEX]),
                handicapColor: data[COLOR_HANDICAP_INDEX]
            );
        }

        /// <summary>
        /// Crée une partie de matchmaking
        /// </summary>
        private Game CreateMatchmakingGame()
        {
            return GameFactory.CreateMatchmakingGame();
        }

        /// <summary>
        /// Stocke la partie dans la collection appropriée
        /// </summary>
        private void StoreGame(Game game, int id, GameType gameType)
        {
            if (gameType == GameType.CUSTOM)
            {
                Server.CustomGames[id] = game;
                availableGameManager.InsertAvailableGame(game);
            }
            else
            {
                Server.MatchmakingGames[id] = game;
            }
        }

        /// <summary>
        /// Définit la réponse à renvoyer au client
        /// </summary>
        private void SetResponse(int id, ref string response, ref string type)
        {
            response = $"{id}-";
            type = "Send_";
        }
    }
}
