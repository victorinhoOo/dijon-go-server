
namespace Server.Model.Images
{
    /// <summary>
    /// Classe responsable du téléchargement et du renommage de fichiers en local.
    /// </summary>
    public class LocalFileUploader : IFileUploader
    {
        private string profilePicsPath;

        public LocalFileUploader(string profilePicsPath)
        {
            this.profilePicsPath = profilePicsPath;
        }

        /// <summary>
        /// Télécharge une photo de profil et la sauvegarde dans le répertoire local.
        /// </summary>
        /// <param name="file">Le fichier à télécharger.</param>
        /// <param name="fileName">Le nom du fichier.</param>
        public void UploadProfilePic(IFormFile file, string fileName)
        {
            try
            {
                // Détermine le chemin complet du fichier
                string filePath = Path.Combine(profilePicsPath, fileName + Path.GetExtension(file.FileName));

                // Sauvegarde le fichier sur le disque local
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'upload de la photo de profil : {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Renomme une photo de profil dans le répertoire local.
        /// </summary>
        /// <param name="oldFileName">L'ancien nom du fichier.</param>
        /// <param name="newFileName">Le nouveau nom du fichier.</param>
        public void RenameProfilePic(string oldFileName, string newFileName)
        {
            try
            {
                // Cherche l'extension du fichier à renommer
                var oldFilePath = Directory.GetFiles(profilePicsPath, oldFileName + ".*").FirstOrDefault();

                if (oldFilePath != null)
                {
                    // Détermine le nouveau chemin avec l'extension actuelle
                    string newFilePath = Path.Combine(profilePicsPath, newFileName + Path.GetExtension(oldFilePath));

                    // Renomme le fichier
                    File.Move(oldFilePath, newFilePath);
                }
                else
                {
                    throw new FileNotFoundException($"Le fichier {oldFileName} n'a pas été trouvé.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du renommage de la photo de profil : {ex.Message}", ex);
            }
        }

        /// <summary>
        /// récupère une photo de profil dans le répertoire local.
        /// </summary>
        /// <param name="fileName">Le nom du fichier</param>
        /// <returns>La photo de profil en bytes</returns>
        /// <exception cref="FileNotFoundException">Levée si l'image n'existe pas dans le dossier local</exception>
        public byte[] GetProfilePic(string fileName)
        {
            string filePath = Directory.GetFiles(profilePicsPath, $"{fileName}.*").FirstOrDefault();

            if (filePath != null)
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                throw new FileNotFoundException("Image de profil non trouvée.");
            }
        }
    }
}
