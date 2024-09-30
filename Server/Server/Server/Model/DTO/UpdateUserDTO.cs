namespace Server.Model.DTO
{
    public class UpdateUserDTO
    {
        private string tokenuser;
        private string? username;
        private string? email;
        private string? password;
        private IFormFile? profilePic;

        public string Tokenuser { get => tokenuser; set => tokenuser = value; }
        public string? Username { get => username; set => username = value; }
        public string? Email { get => email; set => email = value; }
        public string? Password { get => password; set => password = value; }
        public IFormFile? ProfilePic { get => profilePic; set => profilePic = value; }
    }
}
