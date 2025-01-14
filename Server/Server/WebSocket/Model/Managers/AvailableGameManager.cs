using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;

namespace WebSocket.Model.Managers
{
    /// <summary>
    /// Gère les opérations liées aux parties disponibles (personnalisées)
    /// </summary>
    public class AvailableGameManager
    {
        private IGameDAO gameDAO;
        public AvailableGameManager()
        {
            gameDAO = new GameDAO();
        }

        /// <summary>
        /// Insère une partie disponible en base de données (appelée lors de la création d'une partie personnalisée)
        /// </summary>
        /// <param name="game">La partie à insérer</param>
        public void InsertAvailableGame(Game game)
        {
            gameDAO.InsertAvailableGame(game);
        }

        /// <summary>
        /// Supprime une partie disponible en base de données (appelee lors de la suppression d'une partie personnalisée)
        /// </summary>
        /// <param name="id">La partie à supprimer</param>
        public void DeleteAvailableGame(int id)
        {
            gameDAO.DeleteAvailableGame(id);
        }

    }
}

