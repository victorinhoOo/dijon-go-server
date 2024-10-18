using Server.Model.Data;
using System.Security.Cryptography;
using System.Text;

namespace Server.Model.Managers
{
    /// <summary>
    /// Gère la création et la vérification des jetons d'utilisateur.
    /// </summary>
    public class TokenManager
    {
        private readonly ITokenDAO tokenDAO;
        private ILogger<TokenManager> logger;

        public TokenManager(ITokenDAO tokenDAO, ILogger<TokenManager> logger)
        {
            this.tokenDAO = tokenDAO;
            this.logger = logger;
        }

        /// <summary>
        /// Vérifie si le token est valide pour l'utilisateur donné.
        /// </summary>
        /// <param name="user">L'utilisateur.</param>
        /// <param name="token">Le token à vérifier.</param>
        /// <returns>True si le token est valide, sinon False.</returns>
        public bool CheckToken(User user, string token)
        {
            return this.tokenDAO.CheckToken(user.Username, token);
        }

        /// <summary>
        /// Crée un token pour l'utilisateur donné.
        /// </summary>
        /// <param name="user">L'utilisateur.</param>
        /// <returns>Le jeton créé ou "Token non généré" en cas d'échec.</returns>
        public string CreateTokenUser(User user)
        {
            string result = "Token non généré";
            string token = this.GenerateTokenUser();
            if (this.tokenDAO.InsertTokenUser(user.Username, token))
            {
                result = token;
            }
            return result;
        }

        /// <summary>
        /// Récupère l'utilisateur associé au token donné.
        /// </summary>
        /// <param name="token">Le token.</param>
        /// <returns>L'utilisateur associé au token, ou null si le token n'est pas valide.</returns>
        public User GetUserByToken(string token)
        {
            return this.tokenDAO.GetUserByToken(token);
        }

        /// <summary>
        /// Génère un jeton d'utilisateur aléatoire.
        /// </summary>
        /// <returns>Le jeton d'utilisateur généré.</returns>
        private string GenerateTokenUser()
        {
            // Génère un tableau de bytes et le remplit avec des valeurs aléatoires
            byte[] bytes = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            // construit le token à partir des bytes générés
            StringBuilder tokenBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                tokenBuilder.Append(b.ToString("x2"));
            }
            string token = tokenBuilder.ToString();
            logger.LogInformation("Token généré : " + token);
            return token;
        }

        /// <summary>
        /// Constructeur vide pour les tests unitaires
        /// </summary>
        public TokenManager() {}
    }
}
