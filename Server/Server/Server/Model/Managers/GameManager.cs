﻿using Server.Model.Data;
using Server.Model.DTO;

namespace Server.Model.Managers
{
    /// <summary>
    /// Gère les requêtes en lien avec les parties de jeu.
    /// </summary>
    public class GameManager
    {
        private readonly IGameDAO gameDAO;
        public GameManager(IGameDAO gameDAO)
        {
            this.gameDAO = gameDAO;
        }

        /// <summary>
        /// Donne la liste des parties disponibles
        /// </summary>
        /// <returns>Liste d'informations de parties</returns>
        public List<AvailableGameInfoDTO> GetAvailableGames()
        {
            return gameDAO.GetAvailableGames();
        }

        /// <summary>
        /// Renvoi la liste des parties jouées par un joueur
        /// </summary>
        /// <param name="token">Le token du joueur</param>
        /// <returns>Liste de parties</returns>
        public List<GameInfoDTO> GetGamesByToken(string token)
        {
            return gameDAO.GetGamesByToken(token);
        }

        /// <summary>
        /// Renvoi la liste des coups / états de jeu d'une partie
        /// </summary>
        /// <param name="gameId">L'id de la partie que l'on souhaite récupérer</param>
        /// <returns>Liste des états de la partie</returns>
        public List<GameStateDTO> GetGameStatesByGameId(int gameId)
        {
            return gameDAO.GetGameStatesByGameId(gameId);
        }
    }
}
