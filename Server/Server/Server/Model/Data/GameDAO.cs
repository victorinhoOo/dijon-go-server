using Server.Model.Data;
using Server.Model.DTO;
using System.Data;

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
    public List<GameInfoDTO> GetAvailableGames()
    {
        List<GameInfoDTO> result = new List<GameInfoDTO>();

        database.Connect();

        string query = "SELECT id, title, size FROM availablegame;";

        var dataTable = database.ExecuteQuery(query, null);

        // Boucle sur les résultats pour créer des GameInfoDTO
        foreach (DataRow row in dataTable.Rows)
        {
            var gameInfo = new GameInfoDTO
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Size = Convert.ToInt32(row["size"])
            };
            result.Add(gameInfo);
        }

        database.Disconnect();
        logger.LogInformation("Liste des parties récupérée");
        return result;
    }
}
