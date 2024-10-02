namespace Server.Model.Images
{

    /// <summary>
    /// Interface pour le téléchargement de fichiers.
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Télécharge une photo de profil.
        /// </summary>
        /// <param name="file">Le fichier à télécharger.</param>
        /// <param name="username">Le nom d'utilisateur associé à la photo de profil.</param>
        /// <returns>Le chemin d'accès de la photo de profil téléchargée.</returns>
        public void UploadProfilePic(IFormFile file, string username);
    }
}
