using WebSocket.Strategy.Enumerations;
using WebSocket.Model;
using WebSocket.Model.Managers;

namespace WebSocket.Strategy
{
    public class ChatStrategy : IStrategy
    {
        private MessageManager messageManager = new MessageManager();
        public void Execute(Client sender, string[] data, GameType gameType, ref string response, ref string type)
        {
            string recipient = data[2];
            string message = data[3];

            // Vérifie si le destinataire existe
            if (Server.ConnectedClients.TryGetValue(recipient, out _))
            {
                response = $"0-Chat-{sender.User.Name}-{message}";
                type = $"Private-{recipient}_";
                messageManager.AddMessage(sender.User.Name, recipient, message);
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
