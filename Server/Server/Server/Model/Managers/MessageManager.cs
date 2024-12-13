using Server.Model.Data;
using Server.Model.DTO;

namespace Server.Model.Managers
{
    /// <summary>
    /// Gère les messages entre les joueurs.
    /// </summary>
    public class MessageManager
    {
        private IMessageDAO messageDAO;
        private ITokenDAO tokenDAO;
        private ILogger<MessageManager> logger;
        public MessageManager(ITokenDAO tokenDAO, IMessageDAO messageDAO, ILogger<MessageManager> logger)
        {
            this.messageDAO = messageDAO;
            this.tokenDAO = tokenDAO;
            this.logger = logger;
        }
        public List<MessageDTO> GetConversation(string token, string usernameRecipient)
        {
            List<MessageDTO> messages = new List<MessageDTO>();
            User user = tokenDAO.GetUserByToken(token);
            this.logger.LogInformation("Récupération de l'utilisateur " + user.Username + "correspondant à " + token);
            if (user != null)
            {
                messages = messageDAO.GetConversation(user.Username, usernameRecipient);
                this.logger.LogInformation("Récupération de la conversation entre " + user.Username + " et " + usernameRecipient);
            }
            return messages;
        }
    }
}
