using DotNetEnv;
using StackExchange.Redis;

namespace WebSocket.Model.DAO.Redis
{
    /// <summary>
    /// Gère les opérations liées aux états d'une partie dans Redis.
    /// </summary>
    public class GameStateDAO: IGameStateDAO
    {
        private readonly StackExchange.Redis.IDatabase redisDatabase;

        public GameStateDAO()
        {
            string redisConnectionString = Env.GetString("REDIS_CONNECTION_STRING");
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            redisDatabase = redis.GetDatabase();
        }

        /// <inheritdoc/>
        public void AddGameState(GameState gameState)
        {
            string key = $"game:{gameState.GameId}:state";
            string value = GameStateSerializer.Serialize(gameState);
            redisDatabase.ListRightPush(key, value); 
        }

        /// <inheritdoc/>
        public List<GameState> GetGameStates(int gameId)
        {
            string key = $"game:{gameId}:state";
            var states = redisDatabase.ListRange(key);

            return states
                .Select(state => GameStateSerializer.Deserialize(state))
                .ToList();
        }

        /// <inheritdoc/>
        public void DeleteGameStates(int gameId)
        {
            string key = $"game:{gameId}:state";
            redisDatabase.KeyDelete(key);
        }
    }
}
