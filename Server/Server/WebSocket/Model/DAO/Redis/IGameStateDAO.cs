namespace WebSocket.Model.DAO.Redis
{
    /// <summary>
    /// Définit un standard pour les opérations CRUD liées aux états de parties dans la base de données Redis.
    /// </summary>
    public interface IGameStateDAO
    {
        /// <summary>
        /// Ajoute un état de partie  à la base de données Redis.
        /// </summary>
        /// <param name="gameState">L'état de la partie à ajouter.</param>
        public void AddGameState(GameState gameState);

        /// <summary>
        /// Récupère tous les états de parties associés à un identifiant donné.
        /// </summary>
        /// <param name="gameId">L'identifiant de la partie dont on veut récupérer les états.</param>
        /// <returns>Une liste d'états de partie.</returns>
        public List<GameState> GetGameStates(int gameId);

        /// <summary>
        /// Supprime tous les états de partie associés à un identifiant donné.
        /// </summary>
        /// <param name="gameId">L'identifiant de la partie dont on veut supprimer les états.</param>
        public void DeleteGameStates(int gameId);
    }
}
