using Microsoft.AspNetCore.Mvc;
using Server.Model.Managers;

namespace Server.Controllers
{
    /// <summary>
    /// Contrôleur pour l'affichage des images de profil.
    /// </summary>
    [ApiController]
    [Route("profile-pics")]
    public class ImageController : Controller
    {
        private ImageManager imageManager;

        public ImageController(ImageManager imageManager)
        {
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Obtient la photo de profil spécifiée.
        /// </summary>
        /// <param name="fileName">Le nom du fichier de la photo de profil.</param>
        /// <returns>La photo de profil sous forme de fichier.</returns>
        [HttpGet("{fileName}")]
        public IActionResult GetProfilePic(string fileName)
        {
            IActionResult result = NotFound();
            try
            {
                byte[] profilePic = imageManager.GetProfilePic(fileName);
                result = File(profilePic, "image/png");
            }
            catch (Exception ex)
            {
                result = BadRequest(new { Message = ex.Message });
            }
            return result;
        }
    }
}
