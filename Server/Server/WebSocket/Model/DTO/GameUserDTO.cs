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
        private string name;

        private int elo;

        private string token;

        public string Token { get => token; set => token = value; }
        public string Name { get => name; set => name = value; }
        public int Elo { get => elo; set => elo = value; }
    }
}
