using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;

namespace WebSocket
{
    /// <summary>
    /// Interface pour faire l'intermédiaire entre le client et le serveur
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Récupère l'utilisateur associé au client
        /// </summary>
        public GameUserDTO User { get; }

        /// <summary>
        /// Change l'utilisateur associé au client
        /// </summary>
        /// <param name="user">le nouvel utilisateur</param>
        public void ChangeUser(GameUserDTO user);

        /// <summary>
        /// Reçois un message du client
        /// </summary>
        /// <returns>un tableau d'octets contenant la trame qui encapsule le message</returns>
        public byte[] ReceiveMessage();

        /// <summary>
        /// Envoie un message au client
        /// </summary>
        /// <param name="bytes">tableau d'octets contenant la trame qui encapsule le message</param>
        public void SendMessage(byte[] bytes);
    }
}
