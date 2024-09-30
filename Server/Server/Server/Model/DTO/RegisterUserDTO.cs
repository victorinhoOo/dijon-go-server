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
        private IFormFile profilePic;

        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public IFormFile ProfilePic { get => profilePic; set => profilePic = value; }
    }
}
