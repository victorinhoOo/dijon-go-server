using Server.Model.Data;
using Server.Model.DTO;
using System.Data;
using System.Globalization;

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

        string query = "SELECT id, size, rule, creatorName,komi, name, handicap FROM availablegame;";

        var dataTable = database.ExecuteQuery(query, null);

        // Boucle sur les résultats pour créer des GameInfoDTO
        foreach (DataRow row in dataTable.Rows)
        {
            var gameInfo = new AvailableGameInfoDTO
            {
                Id = Convert.ToInt32(row["id"]),
                Size = Convert.ToInt32(row["size"]),
                Rule = row["rule"].ToString(),
                CreatorName = row["creatorName"].ToString(),
                Komi = Convert.ToSingle(row["komi"]),
                Name = row["name"].ToString(),
                Handicap = Convert.ToInt32(row["handicap"])
            };
            result.Add(gameInfo);
        }

        database.Disconnect();
        logger.LogInformation("Liste des parties récupérée");
        return result;
    }
}
