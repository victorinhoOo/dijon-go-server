using Server.Model.Data;
using Server.Model.DTO;
using System.Data;

namespace Server.Model.Data
{

    /// <summary>
    /// Gère les requêtes en lien avec les parties de jeu.
    /// </summary>
    public class GameDAO : IGameDAO
    {
        private readonly IDatabase database;
        private ILogger<GameDAO> logger;

        public GameDAO(IDatabase database, ILogger<GameDAO> logger)
        {
            this.database = database;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public List<AvailableGameInfoDTO> GetAvailableGames()
        {
            List<AvailableGameInfoDTO> result = new List<AvailableGameInfoDTO>();

            database.Connect();

            string query = "SELECT id, title, size, rule FROM availablegame;";

            var dataTable = database.ExecuteQuery(query, null);

            // Boucle sur les résultats pour créer des GameInfoDTO
            foreach (DataRow row in dataTable.Rows)
            {
                var gameInfo = new AvailableGameInfoDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Title = row["title"].ToString(),
                    Size = Convert.ToInt32(row["size"]),
                    Rule = row["rule"].ToString()
                };
                result.Add(gameInfo);
            }

            database.Disconnect();
            logger.LogInformation("Liste des parties récupérée");
            return result;
        }

        /// <inheritdoc/>
        public List<GameInfoDTO> GetGamesByToken(string token)
        {
            List<GameInfoDTO> games = new List<GameInfoDTO>();

            database.Connect();

            try
            {
                string query = @"
                SELECT
                    g.id,
                    g.size,
                    g.rule,
                    g.score_player_1,
                    g.score_player_2,
                    g.date,
                    u1.username AS player1,
                    u2.username AS player2,
                    CASE 
                        WHEN g.winner_id = u.idUser THEN 1
                        ELSE 0
                    END AS won
                FROM savedgame g
                INNER JOIN user u1 ON g.player1_id = u1.idUser
                INNER JOIN user u2 ON g.player2_id = u2.idUser
                INNER JOIN user u ON (u.idUser = g.player1_id OR u.idUser = g.player2_id)
                INNER JOIN tokenuser t ON u.idToken = t.idToken
                WHERE t.token = @token
                ORDER BY g.date DESC";

                var parameters = new Dictionary<string, object>
                {
                    { "@token", token }
                };

                var result = database.ExecuteQuery(query, parameters);

                foreach (DataRow row in result.Rows)
                {
                    GameInfoDTO gameInfo = new GameInfoDTO(
                        Convert.ToInt32(row["id"]),
                        row["player1"].ToString(),
                        row["player2"].ToString(),
                        Convert.ToInt32(row["size"]),
                        row["rule"].ToString(),
                        Convert.ToInt32(row["score_player_1"]),
                        Convert.ToInt32(row["score_player_2"]),
                        Convert.ToBoolean(row["won"]), // Déduit si le joueur a gagné
                        Convert.ToDateTime(row["date"])
                    );

                    games.Add(gameInfo);
                }
            }
            finally
            {
                database.Disconnect();
            }

            return games;
        }

        /// <inheritdoc/>
        public List<GameStateDTO> GetGameStatesByGameId(int gameId)
        {
            List<GameStateDTO> gameStates = new List<GameStateDTO>();

            database.Connect();

            try
            {
                string query = @"
                SELECT 
                    board_state AS Board,
                    captured_black AS CapturedBlack,
                    captured_white AS CapturedWhite
                FROM gamestate
                WHERE game_id = @gameId
                ORDER BY id ASC";

                var parameters = new Dictionary<string, object>
                {
                    { "@gameId", gameId }
                };

                var result = database.ExecuteQuery(query, parameters);

                int moveNumber = 1;

                foreach (DataRow row in result.Rows)
                {
                    GameStateDTO gameState = new GameStateDTO(
                        row["Board"].ToString(),
                        Convert.ToInt32(row["CapturedBlack"]),
                        Convert.ToInt32(row["CapturedWhite"]),
                        moveNumber++
                    );

                    gameStates.Add(gameState);
                }
            }
            finally
            {
                database.Disconnect();
            }

            return gameStates;
        }

    }
}
