using WebSocket.Strategy.Enumerations;
using WebSocket.Model;
using WebSocket.Model.Managers;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Représente la stratégie de chat
    /// </summary>
    public class ChatStrategy : IStrategy
    {
        private readonly MessageManager messageManager = new MessageManager();

        // Constantes pour les indices du tableau data
        private const int DATA_RECIPIENT_INDEX = 2; // Index pour le destinataire
        private const int DATA_MESSAGE_INDEX = 3;   // Index pour le message

        public void Execute(IClient sender, string[] data, GameType gameType, ref string response, ref string type)
        {
            string recipient = data[DATA_RECIPIENT_INDEX];
            string message = data[DATA_MESSAGE_INDEX];

            // Vérifie si le destinataire existe
            if (Server.ConnectedClients.TryGetValue(recipient, out _))
            {
                DateTime timestampMessage = messageManager.AddMessage(sender.User.Name, recipient, message);
                response = $"0-Chat-{sender.User.Name}-{message}-{timestampMessage}";
                type = $"Private-{recipient}_";                
            }
            else
            {
                // En cas d'erreur, on renvoie un message à l'expéditeur
                response = $"Chat-Error-User {recipient} not found";
                type = "Send_";  // Send_ pour renvoyer à l'expéditeur
            }
        }
    }
}
