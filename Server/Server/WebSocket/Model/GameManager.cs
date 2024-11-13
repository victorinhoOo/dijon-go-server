using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;

namespace WebSocket.Model
{
    /// <summary>
    /// Gère les opérations lié aux parties
    /// </summary>
    public class GameManager
    {

        private IUserDAO userDAO;

        public GameManager() 
        {
            this.userDAO = new UserDAO();
        }

        /// <summary>
        /// Récupère un utilisateur à partir de son token
        /// </summary>
        /// <param name="token">token du joueur</param>
        /// <returns>l'utilisateur jouant la partie sous forme de GameUserDTO</returns>
        /// 
        public GameUserDTO GetUserByToken(string token)
        {
            return userDAO.GetUserByToken(token);
        }

        /// <summary>
        /// Modifie l'elo des deux joueurs en fonction du résultat et de leur différence de niveau
        /// </summary>
        /// <param name="winner">L'utilisateur ayant gagné la partie</param>
        /// <param name="looser">L'utilisateur ayant perdu la partie</param>
        public void UpdateEloWinnerLooser(GameUserDTO winner, GameUserDTO looser)
        {
            int initialWinnerElo = winner.Elo;
            int initialLooserElo = looser.Elo;
            this.userDAO.UpdateEloByToken(winner.Token, initialWinnerElo + 10);
            this.userDAO.UpdateEloByToken(looser.Token, initialLooserElo - 10);         
        }
    }
}
