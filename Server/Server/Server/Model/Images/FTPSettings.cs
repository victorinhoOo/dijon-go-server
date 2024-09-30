namespace Server.Model.Images
{
    public class FTPSettings
    {
        private string host;
        private string username;
        private string password;

        public string Host { get => host; set => host = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }

}
