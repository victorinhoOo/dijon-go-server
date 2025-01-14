using GoLogic.Goban;
using GoLogic.Score;
using GoLogic.Serializer;
using GoLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoLogic.Timer;
using WebSocket.Model.Managers;

namespace WebSocket.Model
{
    /// <summary>
    /// Fabrique qui gère la création des parties
    /// </summary>
    public static class GameFactory
    {
        /// <summary>
        /// Crée une partie personnalisée
        /// </summary>
        /// <param name="config">La configuration de la partie</param>
        /// <returns>La partie créee</returns>
        public static Game CreateCustomGame(GameConfiguration config)
        {
            GameBoard gameBoard = new GameBoard(config.Size, config.HandicapColor, config.Handicap);
            GameLogic logic = new GameLogic(gameBoard);
            BoardSerializer boardSerializer = new BoardSerializer(logic);
            ScoreRule scoreRule = ScoreRuleFactory.Create(config.Rule, gameBoard, config.Komi);
            GameManager gameManager = new GameManager();
            TimerManager timerManager = new TimerManager();

            return new Game(config, gameBoard, logic, boardSerializer, scoreRule, gameManager, timerManager);
        }

        /// <summary>
        /// Créé une partie de matchmaking (avec des paramètres par défaut)
        /// </summary>
        /// <returns>La partie créee</returns>
        public static Game CreateMatchmakingGame()
        {
            return CreateCustomGame(GameConfiguration.Default());
        }
    }
}
