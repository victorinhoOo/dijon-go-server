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
        public GameUserDTO GetUsernameByToken(string token)
        {
            return userDAO.GetUserByToken(token);
        }
    }
}
