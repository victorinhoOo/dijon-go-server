using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string GetUsernameByToken(string token);
    }
}
