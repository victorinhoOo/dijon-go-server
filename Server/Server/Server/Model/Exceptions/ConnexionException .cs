namespace Server.Model.Exceptions
{
    /// <summary>
    /// Exception personnalisée levée lors des erreurs de connexion utilisateur.
    /// </summary>
    public class ConnexionException : Exception
    {
        /// <summary>
        /// Initialise une nouvelle instance de l'exception <see cref="ConnexionException"/> avec un message d'erreur spécifié.
        /// </summary>
        /// <param name="message">Le message qui décrit l'erreur.</param>
        public ConnexionException(string message) : base("Erreur lors de la connexion : " + message) { }
    }
}
