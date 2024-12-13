namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Définit un standard pour les opérations liées aux parties dans la base de données.
    /// </summary>
    public interface IGameDAO
    {
        /// <summary>
        /// Insère une partie personnalisée en base de données.
        /// </summary>
        /// <param name="game">Les informations de la partie.</param>
        /// <returns>True si l'insertion a réussi.</returns>
        public bool InsertAvailableGame(Game game);

        /// <summary>
        /// Supprime une partie personnalisée en base de données.
        /// </summary>
        /// <param name="id">L'identifiant de la partie à supprimer.</param>
        public void DeleteAvailableGame(int id);

        /// <summary>
        /// Insère les données d'une partie en base de données.
        /// </summary>
        /// <param name="game">La partie à insérer.</param>
        public void InsertGame(Game game);

        /// <summary>
        /// Met à jour une partie existante dans la base de données.
        /// </summary>
        /// <param name="game">L'objet Game contenant les nouvelles informations à mettre à jour.</param>
        public Task UpdateGameAsync(Game game);

        /// <summary>
        /// Insère l'état d'une partie en base de données.
        /// </summary>
        /// <param name="game">L'état actuel de la partie.</param>
        public void InsertGameState(Game game);

        /// <summary>
        /// Transfère les différents états d'une partie depuis la base de données Redis vers la base de données SQLite.
        /// </summary>
        /// <param name="game">La partie dont les états doivent être transférés.</param>
        public Task TransferMovesToSqliteAsync(Game game);
    }
}
