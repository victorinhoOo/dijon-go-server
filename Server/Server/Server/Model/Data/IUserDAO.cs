namespace Server.Model.Data
{
    /// <summary>
    /// Définit un standard pour l'accès aux données des utilisateurs.
    /// </summary>
    public interface IUserDAO
    {
        /// <summary>
        /// Modifie les informations d'un utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur à modifier.</param>
        /// <returns>True si la modification réussit, sinon False.</returns>
        public bool Update(User user);

        /// <summary>
        /// Connecte un utilisateur (vérifie la concordance du nom d'utilisateur et du mot de passe)
        /// </summary>
        /// <param name="user">L'utilisateur à connecter.</param>
        /// <returns>True si la connexion réussit, sinon False.</returns>
        public bool VerifyExists(User user);

        /// <summary>
        /// Enregistre un nouvel utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur à enregistrer.</param>
        /// <returns>True si l'enregistrement réussit, sinon False.</returns>
        public bool Register(User user);

        /// <summary>
        /// Récupère un utilisateur en fonction de son nom d'utilisateur.
        /// </summary>
        /// <param name="username">Le nom d'utilisateur.</param>
        /// <returns>L'utilisateur correspondant, ou null s'il n'existe pas.</returns>
        public User GetUserByUsername(string username);

        /// <summary>
        /// Renvoie la liste des 5 meilleurs joueurs du serveur
        /// </summary>
        /// <returns>Dictionnaire contenant les 5 joueurs avec le plus d'elo et leur elo respectif</returns>
        public Dictionary<string, int> GetTop5Users();
    }
}
