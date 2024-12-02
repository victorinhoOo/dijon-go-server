using System;
using System.Collections.Generic;

namespace Server.Model.Data
{
    /// <summary>
    /// Classe pour tester les opérations CRUD en lien avec les tokens
    /// </summary>
    public class FakeTokenDAO : ITokenDAO
    {
        // simule des tokens stockés en vdd
        private readonly List<TokenEntry> tokens = new List<TokenEntry>
        {
            new TokenEntry { Username = "victor", Token = "abc123", IdToken = 1 },
            new TokenEntry { Username = "clement", Token = "def456", IdToken = 2 }
        };

        public bool CheckToken(string username, string token)
        {
            // Simule la vérification du token associé à un utilisateur en bdd
            return tokens.Exists(t => t.Username == username && t.Token == token);
        }

        public bool InsertTokenUser(string username, string token)
        {
            // Simule l'insertion d'un nouveau token
            tokens.Add(new TokenEntry { Username = username, Token = token, IdToken = tokens.Count + 1 });
            return true;
        }

        public User GetUserByToken(string token)
        {
            User user = null;
            TokenEntry tokenEntry = tokens.Find(t => t.Token == token);
            if (tokenEntry != null)
            {
                // Retourne un utilisateur factice associé au token
                user = new User(tokenEntry.IdToken, tokenEntry.Username, "mdp", "email@example.com",100);
            }
            return user;
        }

        private class TokenEntry
        {
            public string Username { get; set; }
            public string Token { get; set; }
            public int IdToken { get; set; }
        }
    }
}
