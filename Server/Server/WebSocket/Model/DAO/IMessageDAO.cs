using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Interface pour les accès aux données des messages.
    /// </summary>
    public interface IMessageDAO
    {
        /// <summary>
        /// Insère un nouveau message dans la base de données.
        /// </summary>
        /// <param name="message">Message à insérer.</param>
        public void InsertMessage(MessageDTO message);
    }
}
