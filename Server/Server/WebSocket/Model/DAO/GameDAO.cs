using Server.Model.Data;
using WebSocket.Model.DAO.Redis;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Gère les opérations CRUD liées aux parties en base de données.
    /// </summary>
    public class GameDAO: IGameDAO
    {
        private IDatabase database;
        private IUserDAO userDAO;
        private GameStateDAO gameStateDAO;

        public GameDAO()
        {
            this.database = new SQLiteDatabase("Data Source=../../../../Server/dgs.db");
            this.userDAO = new UserDAO();
            this.gameStateDAO = new GameStateDAO();

        }

        /// <inheritdoc/>
        public bool InsertAvailableGame(Game game)
        {
            this.database.Connect();
            bool res = false;
            string query = "insert into availablegame (id,title,size,rule) values (@id,@title,@size,@rule);";
            var parameters = new Dictionary<string, object>
                {
                    {"@id", game.Id},
                    {"@title", $"Partie numéro {game.Id}"},
                    {"@size", game.Size},
                    {"@rule", game.Rule }
                };
            database.ExecuteNonQuery(query, parameters);
            res = true;
            this.database.Disconnect();
            return res;
        }

        /// <inheritdoc/>
        public void DeleteAvailableGame(int id)
        {
            this.database.Connect();
            string query = "delete from availablegame where id = @id;";
            var parameters = new Dictionary<string, object>
                {
                    {"@id", id}
                };
            database.ExecuteNonQuery(query, parameters);
            this.database.Disconnect();
        }

        /// <inheritdoc/>
        public void InsertGame(Game game)
        {
            this.database.Connect();

            try
            {

                // Préparer la requête SQL
                string query = @"
                    INSERT INTO savedgame 
                    (player1_id, player2_id, size, score_player_1, score_player_2, rule, winner_id, date) 
                    VALUES 
                    (@player1_id, @player2_id, @size, @score_player_1, @score_player_2, @rule, @winner_id, @date);
                ";

                // Paramètres pour la requête
                var parameters = new Dictionary<string, object>
                {
                    {"@player1_id", game.Player1.User.Id},
                    {"@player2_id", game.Player2.User.Id},
                    {"@size", game.Size},
                    {"@score_player_1", DBNull.Value}, // pour le moment le score n'est pas défini
                    {"@score_player_2", DBNull.Value},
                    {"@rule", game.Rule},
                    {"@winner_id", DBNull.Value},
                    {"@date", DateTime.Now} // Date actuelle pour la partie
                };

                // Exécuter la requête
                database.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                // Gérer les erreurs si nécessaire
                Console.WriteLine($"Erreur lors de l'insertion de la partie : {ex.Message}");
                throw;
            }
            finally
            {
                this.database.Disconnect();
            }
        }

        /// <inheritdoc/>
        public void UpdateGame(Game game)
        {
            int winnerId;
            if (game.GetScore().Item1 > game.GetScore().Item2)
            {
                winnerId = game.Player1.User.Id;
            }
            else
            {
                winnerId = game.Player2.User.Id;
            }

            int gameId = GetIdFromGame(game);
            this.database.Connect();

            try
            {
                // Préparer la requête SQL pour la mise à jour
                string query = @"
                    UPDATE savedgame 
                    SET 
                        player1_id = @player1_id,
                        player2_id = @player2_id,
                        size = @size,
                        score_player_1 = @score_player_1,
                        score_player_2 = @score_player_2,
                        rule = @rule,
                        winner_id = @winner_id,
                        date = @date
                    WHERE id = @game_id;
                ";

                // Paramètres pour la requête
                var parameters = new Dictionary<string, object>
                {
                    {"@game_id", gameId},
                    {"@player1_id", game.Player1.User.Id},
                    {"@player2_id", game.Player2.User.Id},
                    {"@size", game.Size},
                    {"@score_player_1", game.GetScore().Item1}, // Récupère le score actuel du joueur 1
                    {"@score_player_2", game.GetScore().Item2}, // Récupère le score actuel du joueur 2
                    {"@rule", game.Rule},
                    {"@winner_id", winnerId}, 
                    {"@date", DateTime.Now} 
                };

                // Exécuter la requête
                database.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(($"Erreur lors de la mise à jour de la partie : {ex.Message}"));
            }
            finally
            {
                this.database.Disconnect();
            }
        }


        private int GetIdFromGame(Game game)
        {
            this.database.Connect();
            int gameId = -1; 

            try
            {
                string query = @"
                    SELECT id 
                    FROM savedgame
                    WHERE player1_id = @player1_id 
                      AND player2_id = @player2_id 
                      AND size = @size 
                      AND rule = @rule
                    ORDER BY id DESC
                    LIMIT 1;
                ";

                var parameters = new Dictionary<string, object>
                {
                    { "@player1_id", game.Player1.User.Id },
                    { "@player2_id", game.Player2.User.Id },
                    { "@size", game.Size },
                    { "@rule", game.Rule }
                };

                var result = this.database.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 0)
                {
                    gameId = Convert.ToInt32(result.Rows[0]["id"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(($"Erreur lors de la récupération de l'ID de la partie : {ex.Message}"));
            }
            finally
            {
                this.database.Disconnect();
            }

            return gameId;
        }

        /// <inheritdoc/>
        public void InsertGameState(Game game)
        {
            GameState gameState = new GameState(
                GetIdFromGame(game),
                game.StringifyGameBoard(),
                game.GetCapturedStone().Item1,
                game.GetCapturedStone().Item2
            );

            gameStateDAO.AddGameState(gameState);
        }

        private void DeleteGameStates(int gameId)
        {
            gameStateDAO.DeleteGameStates(gameId);
        }

        /// <inheritdoc/>
        public void TransferMovesToSqlite(Game game)
        {
            int gameId = GetIdFromGame(game);
            List<GameState> moves = gameStateDAO.GetGameStates(gameId);

            this.database.Connect();

            try
            {
                foreach (var move in moves)
                {
                    string query = @"
                        INSERT INTO gamestate 
                        (game_id, board_state, captured_black, captured_white) 
                        VALUES 
                        (@game_id, @board_state, @captured_black, @captured_white);
                    ";

                    var parameters = new Dictionary<string, object>
                    {
                        {"@game_id", gameId},
                        {"@board_state", move.BoardState},
                        {"@captured_black", move.CapturedBlack},
                        {"@captured_white", move.CapturedWhite}
                    };

                    database.ExecuteNonQuery(query, parameters);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Erreur lors du transfert des coups vers SQLite" + ex.Message);
            }
            finally
            {
                this.database.Disconnect();
            }

            // Supprimer les GameStates dans Redis après le transfert
            DeleteGameStates(gameId);
        }
    }
}
