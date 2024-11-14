using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DTO
{

    /// <summary>
    /// Représente les informations d'un utilisateur jouant une partie de Go
    /// </summary>
    public class GameUserDTO
    {
        private string name;

        private int elo;

        private string token;

        /// <summary>
        /// Renvoi ou modifie le Token d'un utilisateur jouant une partie
        /// </summary>
        public string Token { get => token; set => token = value; }

        /// <summary>
        /// Renvoi le nom d'utilisateur d'un utilisateur jouant une partie
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Renvoi ou modifie l'elo d'un joueur jouant une partie
        /// </summary>
        public int Elo { get => elo; set => elo = value; }

    }
}
