using Server.Model.Data;
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
        public List<GameInfoDTO> GetAvailableGames()
        {
            return gameDAO.GetAvailableGames();
        }
    }
}
