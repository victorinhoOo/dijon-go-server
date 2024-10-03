using FluentFTP;
using FluentFTP.Exceptions;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Server.Model.Images
{
    /// <summary>
    /// Classe responsable de l'envoi des fichiers via FTP.
    /// </summary>
    public class FtpUploader : IFileUploader
    {
        private FtpClient ftpClient;
        public FtpUploader(IOptions<FTPSettings> ftpSettings)
        {
            FTPSettings settings = ftpSettings.Value;
            ftpClient = new FtpClient(settings.Host, settings.Username, settings.Password);
            ftpClient.ValidateCertificate += (control, e) => { e.Accept = true; };
        }

        /// <summary>
        /// Obtient le type de fichier à partir d'un objet IFormFile.
        /// </summary>
        /// <param name="file">Le fichier à analyser.</param>
        /// <returns>Le type de fichier.</returns>
        private string GetFileType(IFormFile file)
        {
            return System.IO.Path.GetExtension(file.FileName).ToLower();
        }
        /// <summary>
        /// Ouvre la connexion au serveur FTP si elle n'est pas déjà ouverte
        /// </summary>
        private void Connect()
        {
            if (!ftpClient.IsConnected)
            {
                ftpClient.AutoConnect();
            }
        }

        /// <summary>
        /// Ferme la connexion au serveur FTP
        /// </summary>
        private void Disconnect()
        {
            if (ftpClient.IsConnected)
            {
                ftpClient.Disconnect();
            }
        }

        /// <inheritdoc/>
        public void RenameProfilePic(string oldFileName, string newFileName)
        {
            try
            {
                this.Connect();
                // Récupère la liste des photos de profil
                string profilePicsDirectory = "/profile_pics/";
                FtpListItem[] profileFiles = ftpClient.GetListing(profilePicsDirectory);
                // Recherche une image dans la liste qui commence par l'ancien nom 
                FtpListItem? oldFile = profileFiles.FirstOrDefault(file => file.Name.StartsWith(oldFileName));

                if (oldFile != null)
                {
                    // on renomme le nom du fichier sur le serveur en gardant son extension
                    string fileType = System.IO.Path.GetExtension(oldFile.Name);
                    string oldFilePath = profilePicsDirectory + oldFile.Name;
                    string newFilePath = profilePicsDirectory + newFileName + fileType;
                    ftpClient.Rename(oldFilePath, newFilePath);
                }
                else
                {
                    throw new FileNotFoundException($"Aucune image de profil trouvée pour {oldFileName}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du renommage de l'image de profil sur le serveur FTP : {ex.Message}", ex);
            }
            finally
            {
                this.Disconnect();  
            }
        }

        /// <inheritdoc/>
        public void UploadProfilePic(IFormFile file, string fileName)
        {
            try
            {
                this.Connect();
                // récupère l'extension du fichier puis l'enregistre au bon format
                string type = GetFileType(file);
                string remoteFilePath = $"/profile_pics/{fileName}{type}";

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    ftpClient.UploadBytes(fileBytes, remoteFilePath, FtpRemoteExists.Overwrite, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du téléversement des fichiers sur le serveur FTP : {ex.Message}", ex);
            }
            finally
            {
                this.Disconnect();  
            }
        }
    }
}
