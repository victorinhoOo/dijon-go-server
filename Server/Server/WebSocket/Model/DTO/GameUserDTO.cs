using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DTO
{
    /// <summary>
    /// stock les informations du joueur de la partie
    /// </summary>
    public class GameUserDTO
    {
        /// <summary>
        /// Get et Set le nom du joueur
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get et Set l'elo du jpoueur
        /// </summary>
        public int Elo { get; set; }
    }
}
