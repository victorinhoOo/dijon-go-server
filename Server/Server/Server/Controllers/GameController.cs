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
        public GameController(GameManager gameManager)
        {
            this.gameManager = gameManager;
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
            }
            catch(Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }
    }
}
