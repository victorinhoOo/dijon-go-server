namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les informations d'un utilisateur pour la connexion.
    /// </summary>
    public class LoginUserDTO
    {
        private string username;
        private string password;

        /// <summary>
        /// Le nom d'utilisateur de la connexion.
        /// </summary>
        public string Username { get => username; set => username = value; }

        /// <summary>
        /// Le mot de passe associé à l'utilisateur.
        /// </summary>
        public string Password { get => password; set => password = value; }
    }
}
