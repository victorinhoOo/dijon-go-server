using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Protocol
{
    /// <summary>
    /// Interface qui regroupe les méthodes principales d'un protocole web
    /// </summary>
    public interface IWebProtocol
    {
        /// <summary>
        /// Démarage de l'écoute
        /// </summary>
        public void Start();

        /// <summary>
        /// Accepte la connexion d'un client 
        /// </summary>
        /// <returns>Renvoie la connexion du client connecté</returns>
        public TcpClient AcceptClient();

        /// <summary>
        /// Initialisation des communications entre le client et le serveur 
        /// </summary>
        /// <param name="data">Requête envoyé par le client au serveur</param>
        /// <returns></returns>
        public byte[] BuildHandShake(string data);


        /// <summary>
        /// Déchiffrement du message envoyé par le client
        /// </summary>
        /// <param name="bytes">Tableau d'octets représentant la trame qui contient le message</param>
        /// <returns>tableau d'octet contenant le message</returns>
        public byte[] DecryptMessage(byte[] bytes);

        /// <summary>
        /// Construction de la trame pour l'envoi d'un message au client
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <returns>Tableau d'octets représentant la trame contenant le message</returns>
        public byte[] BuildMessage(string message);

        /// <summary>
        /// Construction de la trame confirmant la deconnexion d'un client
        /// </summary>
        /// <param name="code">Code de statut de la déconnexion</param>
        /// <returns>Tableau d'octets représentant la tram de déconnexion</returns>
        public byte[] BuildDeconnection(int code);
    }
}
