using Newtonsoft.Json.Linq;
using Server.Model.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Model.Data
{
    /// <summary>
    /// Classe pour tester les opérations CRUD en lien avec les utilisateurs
    /// </summary>
    public class FakeUserDAO : IUserDAO
    {
        // Liste des utilisateurs en dur pour les tests
        private readonly List<UserEntry> users = new List<UserEntry>
        {
            new UserEntry(1, "victor", "14773232b4a5ced20b3871374056bca2355e7dc225610ced8a9e9bc7d63a7abc", "user1@example.com", 1, 100), // hash de "mdp" + sel
            new UserEntry(2, "user2", "hashedPassword2", "user2@example.com", 2, 100),
            new UserEntry(3, "user3", "hashedPassword3", "user3@example.com", 3, 100),
            new UserEntry(4, "Alice", "hashedPassword4", "alice@example.com", 4, 2000),
            new UserEntry(5, "Bob", "hashedPassword5", "bob@example.com", 5, 1800),
            new UserEntry(6, "Charlie", "hashedPassword6", "charlie@example.com", 6, 1500),
        };

        // Méthode pour mettre à jour les informations d'un utilisateur
        public bool Update(User user)
        {
            bool result = false;
            UserEntry userEntry = users.FirstOrDefault(u => u.IdUser == user.Id);
            if (userEntry != null)
            {
                userEntry.Username = user.Username;
                userEntry.HashPwd = user.Password;
                userEntry.Email = user.Email;
                result = true;
            }
            return result;
        }

        // Méthode pour vérifier si un utilisateur existe dans la bdd (connexion)
        public bool VerifyExists(User user)
        {
            return users.Any(u => u.Username == user.Username && u.HashPwd == user.Password);
        }


        // Méthode pour enregistrer un nouvel utilisateur
        public bool Register(User user)
        {
            if (!VerifyExists(user))
            {
                int newId = users.Max(u => u.IdUser) + 1;  // Génère un nouvel ID utilisateur
                users.Add(new UserEntry(newId, user.Username, user.Password, user.Email, 0, 100));
                return true;
            }
            return false;  // Si l'utilisateur existe déjà
        }

        // Méthode pour obtenir un utilisateur par son nom d'utilisateur
        public User GetUserByUsername(string username)
        {
            User result = null;
            UserEntry userEntry = users.FirstOrDefault(u => u.Username == username);
            if (userEntry != null)
            {
                result = new User(userEntry.IdUser, userEntry.Username, userEntry.HashPwd, userEntry.Email,userEntry.Elo);
            }
            return result;
        }

        // Méthode pour obtenir le top 5 des utilisateurs
        public Dictionary<string, int> GetTop5Users()
        {
            return users
                .OrderByDescending(u => u.Elo) // Trie les utilisateurs par leur Elo de manière décroissante
                .Take(5)
                .ToDictionary(u => u.Username, u => u.Elo); // Cree un dictionnaire avec Username comme clé et Elo comme valeur
        }

        // Classe interne pour représenter les utilisateurs stockés 
        private class UserEntry
        {
            public UserEntry(int idUser, string username, string hashPwd, string email, int idToken, int elo)
            {
                IdUser = idUser;
                Username = username;
                HashPwd = hashPwd;
                Email = email;
                IdToken = idToken;
                Elo = elo;
            }

            public int IdUser { get; set; }
            public string Username { get; set; }
            public string HashPwd { get; set; }
            public string Email { get; set; }
            public int IdToken { get; set; }
            public int Elo { get; set; }
        }
    }
}
