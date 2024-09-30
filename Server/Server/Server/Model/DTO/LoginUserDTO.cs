namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les informations d'un utilisateur pour la connexion.
    /// </summary>
    public class LoginUserDTO
    {
        private string username;
        private string password;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
