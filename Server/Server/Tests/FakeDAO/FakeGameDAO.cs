using Server.Model.Data;
using Server.Model.DTO;
using System.Collections.Generic;

namespace Server.Model.Data
{
    /// <summary>
    /// FakeDAO pour les tests de la gestion des parties
    /// </summary>
    public class FakeGameDAO : IGameDAO
    {
        private readonly List<AvailableGameInfoDTO> availableGames;
        private readonly List<GameInfoDTO> games;

        public FakeGameDAO()
        {
            availableGames = new List<AvailableGameInfoDTO>
            {
                new AvailableGameInfoDTO(1, 19, "Classic", "Alice", 6.5f, "Game1", 0, "None"),
                new AvailableGameInfoDTO(2, 13, "Blitz", "Bob", 0.0f, "Game2", 0, "None")
            };

            games = new List<GameInfoDTO>
            {
                new GameInfoDTO(1, "Alice", "Bob", 19, "Classic", 0, 0, false, DateTime.Now),
                new GameInfoDTO(2, "Charlie", "David", 13, "Blitz", 0, 0, true, DateTime.Now)
            };
        }

        public List<AvailableGameInfoDTO> GetAvailableGames()
        {
            return availableGames;
        }

        public GameInfoDTO GetGameById(int id)
        {
            throw new NotImplementedException();
        }

        public List<GameInfoDTO> GetGamesByToken(string token)
        {
            // Simule la récupération des jeux par token
            return games;
        }

        public List<GameStateDTO> GetGameStatesByGameId(int gameId)
        {
            // Simule la récupération des états de jeu
            return new List<GameStateDTO>
            {
                new GameStateDTO("BoardState1", 0, 0, 1),
                new GameStateDTO("BoardState2", 1, 1, 2)
            };
        }

        public int GetLastGameIdByToken(string token)
        {
            throw new NotImplementedException();
        }
    }
} 