using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Définit un standard pour les opérations liées aux utilisateurs dans la bdd
    /// </summary>
    public interface IUserDAO
    {
        /// <summary>
        /// Récupère le nom d'utilisateur associé à un token
        /// </summary>
        /// <param name="token">Le token de l'utilisateur</param>
        /// <returns>Le nom d'utilisateur associé au token</returns>
        public GameUserDTO GetUserByToken(string token);
        
        /// <summary>
        /// update en bd la nouvelle valeur de l'elo du joueur a partir de son nom
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public void UpdateEloByToken(string token, int elo);
    }
}
