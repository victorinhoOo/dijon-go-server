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
        public string GetFileType(IFormFile file)
        {
            return System.IO.Path.GetExtension(file.FileName).ToLower();
        }

        /// <summary>
        /// Téléverse l'image de profil d'un utilisateur sur le serveur FTP, si l'image existe déja, elle est remplacée.
        /// </summary>
        /// <param name="file">Le fichier à téléverser.</param>
        /// <param name="username">Le nom d'utilisateur.</param>
        public void UploadProfilePic(IFormFile file, string username)
        {
            try
            {
                using (ftpClient)
                {

                    // Connexion au serveur FTP
                    ftpClient.AutoConnect();

                    // définit le chemin de l'image sur le serveur (nom de l'utilisateur + extension du fichier)
                    string type = GetFileType(file);
                    string remoteFilePath = $"/profile_pics/{username}{type}";

                    // Lire le contenu du fichier en tableau d'octets
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        // Téléverse le fichier vers le serveur FTP
                        ftpClient.UploadBytes(fileBytes, remoteFilePath, FtpRemoteExists.Overwrite, true);
                    }

                    ftpClient.Disconnect();
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"Erreur lors du téléversement des fichiers sur le serveur FTP : {ex.Message}", ex);
            }
        }

    }
}
