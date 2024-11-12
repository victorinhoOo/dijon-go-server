namespace Server.Model
{
    /// <summary>
    /// Gère les attributs d'un utilisateur
    /// </summary>
    public class User
    {
        private int? id;
        private string username;
        private string password;
        private string email;
        private int? elo;

        public User(int id, string username, string password, string email, int elo)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.email = email;
            this.elo = elo;
        }

        public User() { } // constructeur vide nécessaire pour la récupération depuis la bdd

        /// <summary>
        /// Renvoi ou permet de modifier le nom d'un utilisateur
        /// </summary>
        public string Username { get => username; set => username = value; }

        /// <summary>
        /// Renvoi ou permet de modifier le mot de passe d'un nom d'utilisateur
        /// </summary>
        public string Password { get => password; set => password = value; }

        /// <summary>
        /// Renvoi ou permet de modifier l'email d'un nom d'utilisateur
        /// </summary>
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// Renvoi ou permet de modifier l'id d'un utilisateur
        /// </summary>
        public int? Id { get => id; set => id = value; }

        /// <summary>
        /// Renvoi ou permet de modifier l'id d'un utilisateur
        /// </summary>
        public int? Elo { get => elo; set => elo = value; }
    }
}
