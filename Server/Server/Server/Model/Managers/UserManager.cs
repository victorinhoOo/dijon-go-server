using Microsoft.AspNetCore.Http.HttpResults;
using Server.Model.Data;
using Server.Model.DTO;
using Server.Model.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System;
using Google.Apis.Auth;


namespace Server.Model.Managers
{
    /// <summary>
    /// Gère la logique de gestion des utilisateurs
    /// </summary>
    public class UserManager
    {
        private readonly IUserDAO userDAO;
        private readonly ImageManager imageManager;
        private readonly TokenManager tokenManager;
        private ILogger<UserManager> logger;

        public UserManager(IUserDAO userDAO, ImageManager imageManager, TokenManager tokenManager, ILogger<UserManager> logger)
        {
            this.userDAO = userDAO;
            this.imageManager = imageManager;
            this.tokenManager = tokenManager;
            this.logger = logger;
        }

        /// <summary>
        /// Hash le mot de passe de l'utilisateur puis transmet au DAO la demande de connexion
        /// </summary>
        /// <param name="loginUserDTO">Les informations de connexion de l'utilisateur</param>
        /// <returns>Le token de connexion généré</returns>
        public string Connect(LoginUserDTO loginUserDTO)
        {
            string result = "";

            try
            {
                User user = new User // création d'un objet User pour transmettre les informations de connexion à la bdd
                {
                    Username = loginUserDTO.Username,
                    Password = loginUserDTO.Password
                };
                user.Password = HashPassword(user.Password);
                if (userDAO.VerifyExists(user))
                {
                    result = tokenManager.CreateTokenUser(user); // si la connexion réussit, on génère un token
                    logger.LogInformation("Token: " + result + " créé pour l'utilisateur : " + loginUserDTO.Username);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Erreur lors de la connexion : " + ex.Message);
                throw new Exception("Erreur lors de la connexion : " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Connecte un utilisateur en utilisant un ID token Google.
        /// </summary>
        /// <param name="idToken">L'ID token Google</param>
        /// <returns>Le token de connexion généré</returns>
        public async Task<string> GoogleConnect(string idToken)
        {
            string result = "";
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "995926287687-u9810k8cnmk5b5ifaeh2fmtb4o34kinh.apps.googleusercontent.com" }
            };

            // Récupère les informations de l'utilisateur qui se connecte dans un "payload"
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            User user = new User
            {
                Username = payload.Name,
                Email = payload.Email
            };

            if (userDAO.GetUserByUsername(payload.Name) == null)
            {
                // si l'utilisateur ne s'est jamais connecté avec Google auparavant, on l'enregistre en bdd et on enregistre sa photo de profil
                userDAO.Register(user);
                IFormFile pfp = await imageManager.DownloadImageAsIFormFile(payload.Picture);
                imageManager.UploadProfilePic(pfp, user.Username);
            }

            result = tokenManager.CreateTokenUser(user); // On génère le token pour l'utilisateur connecté

            return result;
        }

        /// <summary>
        /// Hache le mot de passe et enregistre l'utilisateur en base de données
        /// </summary>
        /// <param name="registerUserDto">Les informations d'inscription de l'utilisateur</param>
        public void Register(RegisterUserDTO registerUserDto)
        {
            // Valide les données d'inscription
            if (string.IsNullOrEmpty(registerUserDto.Username) || string.IsNullOrEmpty(registerUserDto.Email) || string.IsNullOrEmpty(registerUserDto.Password))
            {
                logger.LogError("Aucun champ n'a été modifié");
                throw new ArgumentException("Aucun champ n'a été modifié");
            }

            // Vérifie si l'utilisateur existe déjà
            if (userDAO.GetUserByUsername(registerUserDto.Username) != null)
            {

                logger.LogError("Ce nom d'utilisateur existe déjà");
                throw new UserAlreadyExistsException();
            }

            // Créer un nouvel utilisateur pour l'insérer en bdd
            User user = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                Password = HashPassword(registerUserDto.Password) // on hache le mot de passe
            };
            try
            {
                if (registerUserDto.ProfilePic != null) // si une image a été sélectionné par l'utilisateur
                {
                    imageManager.UploadProfilePic(registerUserDto.ProfilePic, user.Username); // Upload l'image de profil sur le serveur
                }
                logger.LogInformation("Inscription de l'utilisateur : " + user.Username);
                userDAO.Register(user);
            }
            catch (System.Exception ex)
            {
                logger.LogError("Erreur lors de l'inscription : " + ex.Message);
                throw new Exception("Erreur lors de l'inscription : " + ex.Message);
            }

        }

        /// <summary>
        /// Met à jour les informations de l'utilisateur en fonction de ce qu'il souhaite modifier
        /// </summary>
        /// <param name="updateUserDTO">Les informations de mise à jour de l'utilisateur</param>
        public void UpdateUser(UpdateUserDTO updateUserDTO)
        {
            // Récupère l'utilisateur existant grâce à son token de connexion 
            User user = tokenManager.GetUserByToken(updateUserDTO.Tokenuser);

            // Vérifie que  que le mot de passe est le bon pour l'utilisateur connecté (pour éviter les usurpations de compte)
            if (this.VerifyPasswordMatchUser(user.Username, updateUserDTO.Oldpassword))
            {
                try
                {
                    //  applique les modifications souhaitées
                    if (updateUserDTO.ProfilePic != null) // l'utilisateur a renseigné une nouvelle photo
                    {
                        imageManager.UploadProfilePic(updateUserDTO.ProfilePic, user.Username);
                    }
                    if (!string.IsNullOrEmpty(updateUserDTO.Username)) // l'utilisateur a renseigné un nouveau nom d'utilisateur
                    {
                        if (this.userDAO.GetUserByUsername(updateUserDTO.Username) == null) // vérifie si le nom d'utilisateur n'est pas déjà pris
                        {
                            imageManager.RenameProfilePic(user.Username, updateUserDTO.Username); // Renomme l'image de profil sur le FTP (pour correspondre au nouveau nom d'utilisateur)
                            user.Username = updateUserDTO.Username;
                        }
                        else
                        {
                            logger.LogError("Ce nom d'utilisateur est déjà pris");
                            throw new Exception("Ce nom d'utilisateur est déjà pris");
                        }
                    }
                    if (!string.IsNullOrEmpty(updateUserDTO.Email))
                    {
                        user.Email = updateUserDTO.Email;
                    }
                    if (!string.IsNullOrEmpty(updateUserDTO.Password))
                    {
                        user.Password = HashPassword(updateUserDTO.Password);
                    }

                    // Enregistre les modifications en base de données 
                    userDAO.Update(user);
                    logger.LogInformation("Utilisateur mis à jour");
                }
                catch (Exception ex)
                {
                    logger.LogError("Erreur  : " + ex.Message);
                    throw new Exception("Erreur  : " + ex.Message);

                }
            }
            else
            {

                logger.LogError("Mot de passe invalide");
                throw new UnauthorizedAccessException("Mot de passe invalide");
            }
        }

        // Vérifie que  que le mot de passe est le bon pour l'utilisateur connecté (pour éviter les usurpations de compte)
        private bool VerifyPasswordMatchUser(string username, string password)
        {
            User user = new User();
            user.Username = username;
            user.Password = this.HashPassword(password);
            return this.userDAO.VerifyExists(user);

        }

        /// <summary>
        /// Renvoie l'utilisateur correspondant au token de connexion
        /// </summary>
        /// <param name="tokenUser">token de connexion de l'utilisateur</param>
        /// <returns>L'utilisateur associé au token</returns>
        /// <exception cref="UnauthorizedAccessException">Levée si le token est invalide</exception>
        public User GetUser(string tokenUser)
        {
            User user = tokenManager.GetUserByToken(tokenUser);
            if (user == null)
            {
                logger.LogError("Utilisateur non trouvé, token invalide");
                throw new UnauthorizedAccessException("Utilisateur non trouvé, token invalide");
            }
            return user;
        }

        /// <summary>
        /// Hash le mot de passe pour le stocker en base de données ou le comparer avec le hash stocké en base de données
        /// (méthode publique pour les tests unitaires)
        /// </summary>
        /// <param name="password">Le mot de passe à hacher</param>
        /// <returns>Le mot de passe haché</returns>
        public string HashPassword(string password)
        {
            // On établit un sel pour renforcer la sécurité du hash
            string salt = "C3ciEst1SuperSelLaBale1Ne";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                string saltedPassword = salt + password;

                // ComputeHash - retourne un tableau de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                // Convertis le tableau de bytes en une chaîne de caractères hexadecimale
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                logger.LogInformation("Mot de passe haché");
                return builder.ToString();
            }
        }
        /// <summary>
        /// Récupère les 5 joueurs les mieux classés
        /// </summary>
        /// <returns>le nom et le classement des 5 meilleurs joueurs</returns>
        public Dictionary<string, int> GetLeaderBoard()
        {
            return this.userDAO.GetTop5Users();
        }
    }
}
