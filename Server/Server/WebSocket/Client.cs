using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DTO;

namespace WebSocket
{
    /// <summary>
    /// Classe qui sert d'intermédiaire entre le client et le serveur
    /// </summary>
    public class Client : IClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private GameUserDTO user;


        public Client(TcpClient client)
        {
            this.client = client;
            this.stream = this.client.GetStream();
            this.user = new GameUserDTO();
        }

        /// <summary>
        /// Récupère l'utilisateur associé au client
        /// </summary>
        public GameUserDTO User { get => user; }

        /// <summary>
        /// Change l'utilisateur associé au client
        /// </summary>
        /// <param name="user">Le nouvel utilisateur</param>
        public void ChangeUser(GameUserDTO user)
        {
            this.user.Token = user.Token;
            this.user.Name = user.Name;
            this.user.Elo = user.Elo;
            this.user.Id = user.Id;
        }

        /// <summary>
        /// Reçois un message du client
        /// </summary>
        /// <returns>un tableau d'octets contenant la trame qui encapsule le message</returns>
        public byte[] ReceiveMessage()
        {
            while (!this.stream.DataAvailable) ; // Tant que le serveur ne reçois rien le serveur reste bloqué à cette étape
            while (this.client.Available < 3) ; // Tant que le serveur n'a pas reçu au moins 3 octets de la part du client le programme attend
            byte[] bytes = new byte[client.Available];
            this.stream.Read(bytes, 0, bytes.Length); // lecture des données envoyées par le client
            return bytes;
        }

        /// <summary>
        /// Envoie un message au client 
        /// </summary>
        /// <param name="bytes">tableau d'octets contenant la trame qui encapsule le message</param>
        public void SendMessage(byte[] bytes)
        {
            try
            {
                this.stream.Write(bytes, 0, bytes.Length);
            }
            catch(Exception ex)
            {
                // On ne fait rien en cas d'exception, si un client se déconnecte on ne veut pas que le serveur crash
            }
        }
    }
}
