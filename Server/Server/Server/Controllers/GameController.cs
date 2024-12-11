using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using Server.Model.DTO;
using Server.Model.Managers;

namespace Server.Controllers
{
    /// <summary>
    /// Sert de point d'entrée pour les requêtes HTTP liées aux parties de jeu.
    /// </summary>
    [ApiController]
    [Route("Games")]
    public class GameController: Controller
    {
        private readonly GameManager gameManager;
        private ILogger<GameController> logger;
        public GameController(GameManager gameManager, ILogger<GameController> logger)
        {
            this.gameManager = gameManager;
            this.logger = logger;
        }


        /// <summary>
        /// Récupère la liste des parties disponibles
        /// </summary>
        /// <returns>Liste des parties avec leur titre et leur id</returns>
        [HttpGet("Available-games")]
        public IActionResult GetAvailableGames()
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer les parties" });
            try
            {
                List<AvailableGameInfoDTO> games = gameManager.GetAvailableGames();
                result = Ok(new { Games = games });
                logger.LogInformation("Liste des parties récupérée");
            }
            catch(Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogInformation("Erreur lors de la récupération de la liste des parties : " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Récupère la liste des parties jouées par un joueur
        /// </summary>
        /// <param name="token">token utilisateur du joueur souhaitant récupérer la liste de ses parties</param>
        /// <returns>Liste des parties</returns>
        [HttpPost("Played-games")]
        public IActionResult GetGamesPlayed(string token)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer les parties" });
            try
            {
                List<GameInfoDTO> games = gameManager.GetGamesByToken(token);
                result = Ok(new { Games = games });
                logger.LogInformation("Liste des parties récupérée");
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogInformation("Erreur lors de la récupération de la liste des parties : " + ex.Message);
            }
            return result;
        }


        /// <summary>
        /// Récupère l'id de la dernière partie jouée par un joueur
        /// </summary>
        /// <param name="token">Token utilisateur du joueur</param>
        /// <returns>L'ID de la dernière partie joué par l'utilisateur</returns>
        [HttpPost("Last-game-id")]
        public IActionResult GetLastGameIdByToken(string token)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer l'ID de la dernière partie" });
            try
            {
                int id = this.gameManager.GetLastGameIdByToken(token);
                result = Ok(new { Id = id });
                logger.LogInformation("ID de la dernière partie récupéré");
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogInformation("Erreur lors de la récupération de l'ID de la dernière partie : " + ex.Message);
            }
            return result;
        }


            /// <summary>
            /// Récupère les informations d'une partie identifiée par un ID
            /// </summary>
            /// <param name="id">ID de la partie dont on veut récupérer les infos</param>
            /// <returns>Les informations de la partie en question</returns>
            [HttpPost("Game")]
        public IActionResult GetGameById(int id)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer la partie" });
            try
            {
                GameInfoDTO game = gameManager.GetGameById(id);
                result = Ok(new { Game = game });
                logger.LogInformation("Partie récupérée");
            }
            catch(Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogInformation("Erreur lors de la récupération de la partie : " + ex.Message);
            }
            return result;
        }

        [HttpPost("Game-states")]
        /// <summary>
        /// Récupère la liste des "états" d'une partie correspondant à chaque coup
        /// </summary>
        /// <param name="idGame">L'id de la partie dont on souhaite récupérer la liste des coups</param>
        /// <returns>Liste des coups / états de la partie</returns>
        public IActionResult GetGameStatesByid(int id)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer les coups de la partie" });
            try
            {
                List<GameStateDTO> gameStates = gameManager.GetGameStatesByGameId(id);
                result = Ok(new { States = gameStates });
                logger.LogInformation("Liste des parties récupérée");
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
                logger.LogInformation("Erreur lors de la récupération des coups de la partie : " + ex.Message);
            }
            return result;
        }

    }
}
