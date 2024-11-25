using Microsoft.AspNetCore.Mvc;
using Server.Model;
using Server.Model.DTO;
using Server.Model.Managers;

namespace Server.Controllers
{
    /// <summary>
    /// Controlleur lié aux utilisateurs
    /// </summary>
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        private readonly UserManager userManager;
        private ILogger<UserController> logger;

        public UserController(UserManager userManager, ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Connexion d'un utilisateur.
        /// </summary>
        /// <param name="loginUserDTO">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Le résultat de la connexion.</returns>
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDTO loginUserDTO)
        {
            IActionResult result = BadRequest(new InvalidLoginException());
            try
            {
                string token = userManager.Connect(loginUserDTO);
                if (token != "")
                {
                    logger.LogInformation("Connexion réussie pour l'utilisateur " + loginUserDTO.Username);
                    result = Ok(new { Token = token });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la connexion : " + ex.Message);
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Connexion d'un utilisateur avec google
        /// </summary>
        /// <param name="idToken">Token de connexion Google de l'utilisateur souhaitant se connecter</param>
        /// <returns>Le résultat de la connexion</returns>
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(string idToken)
        {
            IActionResult result = BadRequest(new InvalidLoginException());
            try
            {
                string token = await userManager.GoogleConnect(idToken);
                if (token != "")
                {
                    logger.LogInformation("Connexion réussie pour l'utilisateur Google");
                    result = Ok(new { Token = token });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la connexion Google : " + ex.Message);
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Inscription d'un utilisateur.
        /// </summary>
        /// <param name="registerUserDTO">Les informations d'inscription de l'utilisateur.</param>
        /// <returns>Le résultat de l'inscription.</returns>
        [HttpPost("Register")]
        public IActionResult Register([FromForm] RegisterUserDTO registerUserDTO)
        {
            IActionResult result = BadRequest(new { Message = "Inscription impossible" });
            try
            {
                userManager.Register(registerUserDTO);
                logger.LogInformation("Inscription réussie pour l'utilisateur " + registerUserDTO.Username);
                result = Ok(new { Message = "Inscription réussie" });

            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de l'inscription : " + ex.Message);
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }


        /// <summary>
        /// Met à jour un utilisateur.
        /// </summary>
        /// <param name="updateUserDTO">Les informations de mise à jour de l'utilisateur.</param>
        /// <returns>Le résultat de la mise à jour.</returns>
        [HttpPost("Update")]
        public IActionResult UpdateUser([FromForm] UpdateUserDTO updateUserDTO)
        {
            IActionResult result = BadRequest(new { Message = "Mise à jour impossible" });
            try
            {
                userManager.UpdateUser(updateUserDTO);
                logger.LogInformation("Mise à jour réussie pour l'utilisateur " + updateUserDTO.Username);
                result = Ok(new { Message = "Mise à jour réussie" });
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la mise à jour : " + ex.Message);
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Renvoie l'utilisateur correspondant au token de connexion.
        /// </summary>
        /// <param name="tokenUser">Token de connexion utilisateur</param>
        /// <returns>L'utilisateur correspondant</returns>
        [HttpGet("Get")]
        public IActionResult GetUser(string tokenUser)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer l'utilisateur" });
            try
            {
                User user = userManager.GetUser(tokenUser);
                logger.LogInformation("Récupération de l'utilisateur " + user.Username);
                result = Ok(new { User = user });
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la récupération de l'utilisateur : " + ex.Message);
                result = BadRequest(new { Message = ex.Message });

            }
            return result;
        }
        /// <summary>
        /// Renvoi le leaderboard : les 5 meilleurs joueurs en terme d'elo
        /// </summary>
        /// <returns>le nom et le classement des 5 meilleurs joueurs</returns>
        [HttpGet("Leaderboard")]
        public IActionResult GetLeardBoard()
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer le leaderboard" });
            try
            {
                //recuperation du leaderboard
                Dictionary<string, int> leaderboard = this.userManager.GetLeaderBoard();
                logger.LogInformation("Récupération de leaderboard " + leaderboard.Count);
                result = Ok(leaderboard);
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la récupération du leaderboard: " + ex.Message);
                result = BadRequest(new { Message = ex.Message });

            }
            return result;
        }
    }
}
