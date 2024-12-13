using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;

namespace WebSocket.Model.Managers
{
    /// <summary>
    /// Classe permettant de gérer les messages.
    /// </summary>
    public class MessageManager
    {
        private IMessageDAO messageDAO;
        private IUserDAO userDAO;
        public MessageManager()
        {
            messageDAO = new MessageDAO();
            userDAO = new UserDAO();
        }

        /// <summary>
        /// Constructeur pour les tests
        /// </summary>
        /// <param name="messageDAO"></param>
        /// <param name="userDAO"></param>
        public MessageManager(IMessageDAO messageDAO, IUserDAO userDAO)
        {
            this.messageDAO = messageDAO;
            this.userDAO = userDAO;
        }

        /// <summary>
        /// Ajoute un message à la base de données.
        /// </summary>
        /// <param name="senderUserName">Le nom d'utilisateur de l'envoyeur du message</param>
        /// <param name="receiverUserName">Le nom d'utilisateur du receveur du message</param>
        /// <param name="content">le contenu du message</param>
        public DateTime AddMessage(string senderUserName, string receiverUserName, string content)
        {
            int idSender = this.userDAO.GetIdByUsername(senderUserName);
            int idReceiver = this.userDAO.GetIdByUsername(receiverUserName);
            DateTime timestampMessageCreated = DateTime.Now;
            MessageDTO message = new MessageDTO(idSender, idReceiver, content, timestampMessageCreated);
            messageDAO.InsertMessage(message);
            return timestampMessageCreated;
        }
    }
}
