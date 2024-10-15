using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using Server.Model.DTO;

namespace Server.Controllers
{
    [ApiController]
    [Route("Games")]
    public class GameController: Controller
    {
        public GameController()
        {
        }

        /// <summary>
        /// Récupère la liste des parties disponibles
        /// </summary>
        /// <returns>Liste des parties avec leur titre et leur id</returns>
        [HttpGet("Available-games")]
        public IActionResult GetAvailableGames()
        {
            List<GameInfoDTO> games = new List<GameInfoDTO>
            {
                new GameInfoDTO { Id = 1, Title = "Partie de user1", Size= 9 },
                new GameInfoDTO { Id = 2, Title = "Partie de user2", Size = 12 },
                new GameInfoDTO { Id = 3, Title = "Partie de user3", Size = 9 },
                new GameInfoDTO { Id = 4, Title = "Partie de user4", Size = 19 }
            }; 
            return Ok(new { Games = games });
        }
    }
}
