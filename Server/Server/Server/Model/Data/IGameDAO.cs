using Server.Model.DTO;

namespace Server.Model.Data
{
    /// <summary>
    /// Définit un standard pour les classes qui gèrent les parties en base de données.
    /// </summary>
    public interface IGameDAO
    {
        /// <summary>
        /// Récupère la liste des parties disponibles 
        /// </summary>
        /// <returns>Liste de parties</returns>
        public List<AvailableGameInfoDTO> GetAvailableGames();

        /// <summary>
        /// Récupère la liste des parties jouées par un joueur
        /// </summary>
        /// <param name="token">Token utilisateur du joueur</param>
        /// <returns>Liste de parties</returns>
        public List<GameInfoDTO> GetGamesByToken(string token);

        /// <summary>
        /// Récupère l'id de la dernière partie jouée par un joueur
        /// </summary>
        /// <param name="token">Token utilisateur du joueur</param>
        /// <returns>L'ID de la dernière partie joué par l'utilisateur</returns>
        public int GetLastGameIdByToken(string token);

        /// <summary>
        /// Récupère la liste des états de la partie (les coups) pour une partie donnée.
        /// </summary>
        /// <param name="gameId">L'identifiant de la partie.</param>
        /// <returns>Liste des états de la partie sous forme de GameStateDTO.</returns>
        public List<GameStateDTO> GetGameStatesByGameId(int gameId);

        /// <summary>
        /// Rcupère les informations d'une partie en fonction de son ID
        /// </summary>
        /// <param name="id">ID de la partie à récupérer</param>
        /// <returns>Les informations de la partie en question</returns>
        public GameInfoDTO GetGameById(int id);
    }
}
