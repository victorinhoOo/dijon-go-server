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
        /// <param name="fileName">Le nom du fichier.</param>
        /// <returns>Le chemin d'accès de la photo de profil téléchargée.</returns>
        public void UploadProfilePic(IFormFile file, string fileName);

        /// <summary>
        /// Renomme une photo de profil
        /// </summary>
        /// <param name="oldFileName">L'ancien nom du fichier</param>
        /// <param name="newFileName">Le nouveau nom du fichier</param>
        public void RenameProfilePic(string oldFileName, string newFileName);

        /// <summary>
        /// Récupère une photo de profil
        /// </summary>
        /// <param name="fileName">Nom du fichier à récupérer</param>
        /// <returns>Le fichier récupéré</returns>
        public byte[] GetProfilePic(string fileName);
    }
}
