using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;

namespace Tests.WebSockets.FakeDAO
{
    /// <summary>
    /// FakeDAO pour les tests de la websocket
    /// </summary>
    public class FakeUserDAO : IUserDAO
    {
        private readonly Dictionary<string, GameUserDTO> users;

        public FakeUserDAO()
        {
            // Données utilisateurs en dur pour les tests
            users = new Dictionary<string, GameUserDTO>
        {
            { "token1", new GameUserDTO { Id = 1, Name = "Alice", Elo = 2000, Token = "token1" } },
            { "token2", new GameUserDTO { Id = 2, Name = "Bob", Elo = 1800, Token = "token2" } },
            { "token3", new GameUserDTO { Id = 3, Name = "Charlie", Elo = 1500, Token = "token3" } },
            { "token4", new GameUserDTO { Id = 4, Name = "victor", Elo = 100, Token = "token4" } },
            { "token5", new GameUserDTO { Id = 5, Name = "user2", Elo = 100, Token = "token5" } }
        };
        }

        public GameUserDTO GetUserByToken(string token)
        {
            users.TryGetValue(token, out var user);
            return user;
        }

        public void UpdateEloByToken(string token, int newElo)
        {
            if (users.ContainsKey(token))
            {
                users[token].Elo = newElo;
            }
        }

        public List<KeyValuePair<string, int>> GetTop5Users()
        {
            return users.Values
                .OrderByDescending(u => u.Elo)
                .Take(5)
                .Select(u => new KeyValuePair<string, int>(u.Name, u.Elo))
                .ToList();
        }

        public int GetIdByUsername(string username)
        {
            var user = users.Values.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
            return user?.Id ?? -1;
        }
    }
}
