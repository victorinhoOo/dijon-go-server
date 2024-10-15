namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les données d'inscription d'un utilisateur.
    /// </summary>
    public class RegisterUserDTO
    {
        private string username;
        private string email;
        private string password;
        private IFormFile? profilePic;

        /// <summary>
        /// Le nom de l'utilisateur souhaitant s'inscrire
        /// </summary>
        public string Username { get => username; set => username = value; }

        /// <summary>
        /// Le mail de l'utilisateur souhaitant s'inscrire
        /// </summary>
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// Le mot de passe de l'utilisateur souhaitant s'inscrire
        /// </summary>
        public string Password { get => password; set => password = value; }

        /// <summary>
        /// La photo de profil de l'utilisateur souhaitant s'inscrire
        /// </summary>
        public IFormFile? ProfilePic { get => profilePic; set => profilePic = value; }
    }
}
