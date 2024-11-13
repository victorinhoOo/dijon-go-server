using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;

namespace WebSocket.Model
{
    public class GameManager
    {

        private IUserDAO userDAO;

        /// <summary>
        /// contructeur du game manager
        /// </summary>
        public GameManager() 
        {
            this.userDAO = new UserDAO();
        }
        /// <summary>
        /// Récuppérer le pseudo du joueur à partir de son token
        /// </summary>
        /// <param name="token">token du joueur</param>
        /// <returns>le pseudo du joueur</returns>
        public GameUserDTO GetUserByToken(string token)
        {
            return userDAO.GetUserByToken(token);
        }
        /// <summary>
        /// augmente l'elo du joueur de la partie
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="looser"></param>
        /// <returns></returns>
        public void UpdateEloUserByUserName(GameUserDTO winner, GameUserDTO looser)
        {
            this.userDAO.UpdateEloByToken(winner.Name, winner.Elo + 10);
            this.userDAO.UpdateEloByToken(looser.Name, looser.Elo - 10);
            
        }
    }
}
