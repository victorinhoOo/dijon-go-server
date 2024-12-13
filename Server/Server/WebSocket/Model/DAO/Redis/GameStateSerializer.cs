using Newtonsoft.Json;

namespace WebSocket.Model.DAO.Redis
{
    /// <summary>
    /// Classe servant à sérialiser / désérialiser des objets GameState
    /// </summary>
    public class GameStateSerializer
    {
        /// <summary>
        /// Sérialise un objet GameState en JSON.
        /// </summary>
        /// <returns>Une chaîne JSON représentant l'objet.</returns>
        public static string Serialize(GameState gameState)
        {
            return JsonConvert.SerializeObject(gameState);
        }

        /// <summary>
        /// Désérialise une chaîne JSON en un objet GameState.
        /// </summary>
        /// <param name="json">La chaîne JSON à désérialiser.</param>
        /// <returns>Un objet GameState désérialisé.</returns>
        public static GameState Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GameState>(json);
        }
    }
}
