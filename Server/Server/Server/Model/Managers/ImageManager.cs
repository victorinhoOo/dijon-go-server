using FluentFTP;
using Microsoft.Extensions.Options;
using Server.Model.Images;

namespace Server.Model.Managers
{
    /// <summary>  
    /// Gère l'upload et la modification d'images  
    /// </summary>  
    public class ImageManager
    {
        private readonly IFileUploader fileUploader;

        public ImageManager(IFileUploader fileUploader)
        {
            this.fileUploader = fileUploader;
        }


        /// <summary>  
        /// Télécharge une photo de profil.  
        /// </summary>  
        /// <param name="file">Le fichier de la photo de profil.</param>  
        /// <param name="username">Le nom d'utilisateur associé à la photo de profil.</param>  
        public void UploadProfilePic(IFormFile file, string username)
        {
            fileUploader.UploadProfilePic(file, username);
        }
    }
}
