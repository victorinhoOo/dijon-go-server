using WebSocket.Strategy.Enumerations;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Interface définissant une stratégie de traitement des messages WebSocket
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Exécute la stratégie de traitement pour un message reçu
        /// </summary>
        /// <param name="player">Le client qui a envoyé le message</param>
        /// <param name="data">Les données du message sous forme de tableau de chaînes</param>
        /// <param name="gameType">Le type de partie concernée</param>
        /// <param name="response">La réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Le type de réponse (modifié par référence)</param>
        public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type);
    }
}
