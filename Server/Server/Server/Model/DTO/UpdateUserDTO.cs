namespace Server.Model.DTO
{
    /// <summary>
    /// Représente les données de modification d'un utilisateur.
    /// </summary>
    public class UpdateUserDTO
    {
        private string tokenuser;
        private string? username;
        private string? email;
        private string oldpassword;
        private string? password;
        private IFormFile? profilePic;

        /// <summary>
        /// Le token associé à l'utilisateur connecté souhaitant modifier ses informations.
        /// </summary>
        public string Tokenuser { get => tokenuser; set => tokenuser = value; }

        /// <summary>
        /// Le nouveau nom d'utilisateur de l'utilisateur connecté.
        /// </summary>
        public string? Username { get => username; set => username = value; }

        /// <summary>
        /// Le nouvel email de l'utilisateur connecté.
        /// </summary>
        public string? Email { get => email; set => email = value; }

        /// <summary>
        /// Le nouveau mot de passe de l'utilisateur connecté.
        /// </summary>
        public string? Password { get => password; set => password = value; }

        /// <summary>
        /// La nouvelle photo de profil de l'utilisateur connecté.
        /// </summary>
        public IFormFile? ProfilePic { get => profilePic; set => profilePic = value; }

        /// <summary>
        /// L'ancien mot de passe de l'utilisateur 
        /// </summary>
        public string Oldpassword { get => oldpassword; set => oldpassword = value; }
    }
}
