using WebSocket.Model.DTO;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Définit un standard pour les opérations liées aux utilisateurs dans la bdd
    /// </summary>
    public interface IUserDAO
    {
        /// <summary>
        /// Récupère l'utilisateur associé à un token
        /// </summary>
        /// <param name="token">Le token de l'utilisateur</param>
        /// <returns>L'utilisateur associé au token</returns>
        public GameUserDTO GetUserByToken(string token);
        
        /// <summary>
        /// Met à jour l'elo d'un utilisateur à partir de son token
        /// </summary>
        /// <param name="token">Token de l'utilisateur</param>
        /// <param name="elo">Le nouvel elo de l'utilisateur</param>
        public void UpdateEloByToken(string token, int elo);

        /// <summary>
        /// Récupère l'id d'un utilisateur à partir de son nom d'utilisateur
        /// </summary>
        /// <param name="username">le nom de l'utilisateur</param>
        /// <returns>l'id correspondant en bdd</returns>
        public int GetIdByUsername(string username);
    }
}
