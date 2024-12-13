using DotNetEnv;
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
            string sqliteConnectionString = Env.GetString("SQLITE_CONNECTION_STRING");
            this.database = new SQLiteDatabase(sqliteConnectionString);
            this.userDAO = new UserDAO();
            this.gameStateDAO = new GameStateDAO();

        }

        /// <inheritdoc/>
        public bool InsertAvailableGame(Game game)
        {
            this.database.Connect();
            bool res = false;
            string query = "insert into availablegame (id,size,rule,komi,name,creatorName,handicap,handicapColor) values (@id,@size,@rule,@komi,@name,@creatorName,@handicap,@handicapColor);";
            var parameters = new Dictionary<string, object>
                {
                    {"@id", game.Id},
                    {"@size", game.Config.Size},
                    {"@rule", game.Config.Rule },
                    {"@komi", game.Config.Komi },
                    {"@name", game.Config.Name },
                    {"@creatorName",game.Player1.User.Name },
                    {"@handicap",game.Config.Handicap },
                    {"@handicapColor",game.Config.HandicapColor },

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
                    {"@size", game.Config.Size},
                    {"@score_player_1", 0}, // pour le moment le score n'est pas défini
                    {"@score_player_2", 0},
                    {"@rule", game.Config.Rule},
                    {"@winner_id", game.Player1.User.Id},
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
        public async Task UpdateGameAsync(Game game)
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
            database.Connect(); // Connecter de manière asynchrone

            try
            {
                // Préparer la requête SQL pour la mise à jour
                string query = @"
                    UPDATE savedgame 
                    SET 
                        score_player_1 = @score_player_1,
                        score_player_2 = @score_player_2,
                        winner_id = @winner_id,
                        date = @date
                    WHERE id = @game_id;
                ";

                // Paramètres pour la requête
                var parameters = new Dictionary<string, object>
                {
                    {"@game_id", gameId},
                    {"@score_player_1", game.GetScore().Item1}, // Récupère le score actuel du joueur 1
                    {"@score_player_2", game.GetScore().Item2}, // Récupère le score actuel du joueur 2
                    {"@winner_id", winnerId},
                    {"@date", DateTime.Now}
                };

                // Exécuter la requête de manière asynchrone
                database.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour de la partie : {ex.Message}");
            }
            finally
            {
                database.Disconnect(); // Déconnexion asynchrone
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
                    ORDER BY id DESC
                    LIMIT 1;
                ";

                var parameters = new Dictionary<string, object>
                {
                    { "@player1_id", game.Player1.User.Id },
                    { "@player2_id", game.Player2.User.Id },
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
        public async Task TransferMovesToSqliteAsync(Game game)
        {
            int gameId = GetIdFromGame(game);
            List<GameState> moves = gameStateDAO.GetGameStates(gameId); 

            database.Connect(); 

            try
            {
                foreach (GameState move in moves)
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

                    // Exécuter la requête de manière asynchrone
                    database.ExecuteNonQuery(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du transfert des coups vers SQLite : {ex.Message}");
            }
            finally
            {
                database.Disconnect(); // Déconnexion asynchrone
            }

            // Supprimer les GameStates dans Redis après le transfert
            DeleteGameStates(gameId); // Supposant que DeleteGameStates est aussi asynchrone
        }

    }
}
