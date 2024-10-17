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
                List<GameInfoDTO> games = gameManager.GetAvailableGames();
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
    }
}
