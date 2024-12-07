using Server.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO.Redis;
using WebSocket.Model;
using Server.Model.DTO;
using WebSocket.Model.DTO;

namespace Tests.WebSockets.FakeDAO
{
    /// <summary>
    /// FakeDAO pour les tests de la websocket
    /// </summary>
    public class FakeGameDAO : WebSocket.Model.DAO.IGameDAO
    {
        private readonly List<Game> games;
        private readonly List<GameState> gameStates;

        public FakeGameDAO()
        {
            // Données de jeu en dur pour les tests
            games = new List<Game>();
            gameStates = new List<GameState>();
        }

        public bool InsertAvailableGame(Game game)
        {
            games.Add(game);
            return true;
        }

        public void DeleteAvailableGame(int id)
        {
            games.RemoveAll(g => g.Id == id);
        }

        public void InsertGame(Game game)
        {
            games.Add(game);
        }

        public async Task UpdateGameAsync(Game game)
        {
            var existingGame = games.FirstOrDefault(g => g.Id == game.Id);
            if (existingGame != null)
            {
                existingGame.Player1 = game.Player1;
                existingGame.Player2 = game.Player2;
            }
            await Task.CompletedTask;
        }

        public void InsertGameState(Game game)
        {
            var gameState = new GameState(
                game.Id,
                game.StringifyGameBoard(),
                game.GetCapturedStone().Item1,
                game.GetCapturedStone().Item2
            );
            gameStates.Add(gameState);
        }

        public async Task TransferMovesToSqliteAsync(Game game)
        {
            await Task.CompletedTask; // Pas besoin de logique réelle dans le fake
        }

        public List<GameState> GetGameStates(int gameId)
        {
            return gameStates.Where(gs => gs.GameId == gameId).ToList();
        }

        public void DeleteGameStates(int gameId)
        {
            gameStates.RemoveAll(gs => gs.GameId == gameId);
        }

        public List<AvailableGameInfoDTO> GetAvailableGames()
        {
            return games.Select(game => new AvailableGameInfoDTO
            (
                game.Id,
                game.Config.Size,
                game.Config.Rule,
                game.Player1.User.Name,
                6.5f,
                "Ma Game",
                 0,
                 "white"
            )
            ).ToList();
        }



        public List<GameInfoDTO> GetGamesByToken(string token)
        {
            return games.Where(game =>
                    game.Player1.User.Token == token || game.Player2.User.Token == token)
                .Select(game => new GameInfoDTO
                (
                    game.Id,
                    game.Player1.User.Name,
                    game.Player2.User.Name,
                    game.Config.Size,
                    game.Config.Rule,
                    game.GetScore().Item1,
                    game.GetScore().Item2,
                    true,
                    DateTime.Now
                )).ToList();
        }
    }
}
