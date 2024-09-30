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

        public User(int id, string username, string password, string email)
        {
            this.Id = id;
            this.username = username;
            this.password = password;
            this.email = email;
        }

        public User() { }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public int? Id { get => id; set => id = value; }
    }
}
