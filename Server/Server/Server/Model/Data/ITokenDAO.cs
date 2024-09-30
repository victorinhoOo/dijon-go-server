namespace Server.Model.Data
{
    /// <summary>
    /// Interface pour gérer les opérations liées aux tokens.
    /// </summary>
    public interface ITokenDAO
    {
        /// <summary>
        /// Vérifie si le token correspond à l'utilisateur spécifié.
        /// </summary>
        /// <param name="username">Le nom d'utilisateur.</param>
        /// <param name="token">Le token à vérifier.</param>
        /// <returns>True si le token correspond à l'utilisateur, sinon False.</returns>
        public bool CheckToken(string username, string token);

        /// <summary>
        /// Insère un nouveau token pour l'utilisateur spécifié.
        /// </summary>
        /// <param name="username">Le nom d'utilisateur.</param>
        /// <param name="token">Le token à insérer</param>
        /// <returns>True si le token a bien été inséré</returns>
        public bool InsertTokenUser(string username, string token);

        /// <summary>
        /// Récupère l'utilisateur associé au token spécifié.
        /// </summary>
        /// <param name="token">Le token.</param>
        /// <returns>L'utilisateur associé au token.</returns>
        public User GetUserByToken(string token);
    }
}
