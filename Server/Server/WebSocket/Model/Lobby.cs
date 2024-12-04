using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model
{
    public class Lobby
    {
        private int id;
        private Client player1;
        private Client player2;

        public Client Player1 { get => player1; set => player1 = value; }
        public Client Player2 { get => player2; set => player2 = value; }
        public int Id { get => id; set => id = value; }

        public Lobby(int id)
        {
            this.id = id;
        }
    }
}
