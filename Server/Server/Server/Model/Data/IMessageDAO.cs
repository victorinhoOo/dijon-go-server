using Server.Model.DTO;

namespace Server.Model.Data
{
    /// <summary>
    /// Classe permettant d'accéder aux données des messages.
    /// </summary>
    public interface IMessageDAO
    {
        /// <summary>
        /// Permet de récupérer la conversation entre deux utilisateurs.
        /// </summary>
        /// <param name="username1">le nom d'utilisateur du premier utilisateur</param>
        /// <param name="username2">le nom d'utilisateur du deuxième utilisateur</param>
        /// <returns>La liste des messages échangés triés par date</returns>
        public List<MessageDTO> GetConversation(string username1, string username2);
    }
}
