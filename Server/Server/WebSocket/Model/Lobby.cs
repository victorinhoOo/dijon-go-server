using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model
{
    /// <summary>
    /// Représente un lobby de jeu
    /// </summary>
    public class Lobby
    {
        private int id;
        private IClient player1;
        private IClient player2;

        /// <summary>
        /// Récupère ou modifie le joueur 1 du lobby
        /// </summary>
        public IClient Player1 { get => player1; set => player1 = value; }

        /// <summary>
        /// Récupère ou modifie le joueur 2 du lobby
        /// </summary>
        public IClient Player2 { get => player2; set => player2 = value; }

        /// <summary>
        /// Récupère ou modifie l'id du lobby
        /// </summary>
        public int Id { get => id; set => id = value; }

        public Lobby(int id)
        {
            this.id = id;
        }
    }
}
