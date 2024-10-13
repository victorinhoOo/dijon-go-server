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

        public UserController(UserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Connexion d'un utilisateur.
        /// </summary>
        /// <param name="loginUserDTO">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Le résultat de la connexion.</returns>
        [HttpPost("Login")]
        public IActionResult Login([FromForm] LoginUserDTO loginUserDTO)
        {
            IActionResult result = BadRequest(new { Message = "L'utilisateur n'existe pas" });
            try
            {
                string token = userManager.Connect(loginUserDTO);
                if (token != "")
                {
                    result = Ok(new { Token = token });
                }
            }
            catch (Exception ex)
            {
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
                result = Ok(new { Message = "Inscription réussie" });

            }
            catch (Exception ex)
            {
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
                result = Ok(new { Message = "Mise à jour réussie" });
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Renvoie l'utilisateur correspondant au token de connexion.
        /// </summary>
        /// <param name="tokenUser">Token de connexion utilisateur</param>
        /// <returns>L'utilisateur correspondant</returns>
        [HttpPost("GetUser")]
        public IActionResult GetUser(string tokenUser)
        {
            IActionResult result = BadRequest(new { Message = "Impossible de récupérer l'utilisateur" });
            try
            {
                User user = userManager.GetUser(tokenUser);
                result = Ok(new { User = user });
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });

            }
            return result;
        }
    }
}
