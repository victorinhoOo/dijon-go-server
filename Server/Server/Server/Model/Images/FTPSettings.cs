namespace Server.Model.Images
{
    /// <summary>
    /// Représente les paramètres de connexion au serveur FTP pour l'upload de fichiers.
    /// Initialisé dans Program.cs et définit dans appsettings.json
    /// </summary>
    public class FTPSettings
    {
        private string host;
        private string username;
        private string password;

        /// <summary>
        /// L'hôte de connexion au serveur FTP.
        /// </summary>
        public string Host { get => host; set => host = value; }

        /// <summary>
        /// Le nom d'utilisateur pour la connexion au serveur FTP.
        /// </summary>
        public string Username { get => username; set => username = value; }

        /// <summary>
        /// Le mot de passe associé à cet utilisateur
        /// </summary>
        public string Password { get => password; set => password = value; }
    }

}
