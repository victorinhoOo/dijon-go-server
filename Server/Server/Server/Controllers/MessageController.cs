using Microsoft.AspNetCore.Mvc;
using Server.Model.DTO;
using Server.Model.Managers;

namespace Server.Controllers
{
    /// <summary>
    /// Contrôleur gérant les messages entre les joueurs.
    /// </summary>
    [ApiController]
    [Route("Messages")]
    public class MessageController: Controller
    {
        private readonly MessageManager messageManager;
        private ILogger<MessageController> logger;
        public MessageController(MessageManager messageManager, ILogger<MessageController> logger)
        {
            this.messageManager = messageManager;
            this.logger = logger;
        }

        /// <summary>
        /// Récupère la conversation entre deux joueurs.
        /// </summary>
        /// <param name="token">le token utilisateur du client qui fait la demande</param>
        /// <param name="usernameRecipient">le nom d'utilisateur de l'interlocuteur dont il souhaite récupérer la conversation</param>
        /// <returns>Une liste de messages entre les deux joueurs</returns>
        [HttpPost("Conversation")]
        public IActionResult GetConversation(string token, string usernameRecipient)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer les parties" });
            try
            {
                List<MessageDTO> messages = messageManager.GetConversation(token, usernameRecipient);
                this.logger.LogInformation($"Récupération de la conversation entre {token} et {usernameRecipient}");
                result = Ok(new { Messages = messages });
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }
    }
}
