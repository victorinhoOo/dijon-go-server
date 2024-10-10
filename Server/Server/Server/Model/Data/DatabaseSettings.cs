namespace Server.Model.Data
{
    /// <summary>
    /// Représente les paramètres de connexion à la base de données.
    /// Initialisé dans Program.cs et définit dans appsettings.json
    /// </summary>
    public class DatabaseSettings
    {
        private string defaultConnection;

        /// <summary>
        /// String de connexion à la bdd, initialisé dans Program.cs
        /// </summary>
        public string DefaultConnection { get => defaultConnection; set => defaultConnection = value; }
    }
}
