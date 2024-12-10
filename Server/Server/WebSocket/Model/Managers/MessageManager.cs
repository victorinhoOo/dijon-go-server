﻿using System;
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
        /// Ajoute un message à la base de données.
        /// </summary>
        /// <param name="senderUserName">Le nom d'utilisateur de l'envoyeur du message</param>
        /// <param name="receiverUserName">Le nom d'utilisateur du receveur du message</param>
        /// <param name="content">le contenu du message</param>
        public void AddMessage(string senderUserName, string receiverUserName, string content)
        {
            int idSender = this.userDAO.GetIdByUsername(senderUserName);
            int idReceiver = this.userDAO.GetIdByUsername(receiverUserName);
            MessageDTO message = new MessageDTO(idSender, idReceiver, content, DateTime.Now);
            messageDAO.InsertMessage(message);
        }
    }
}
