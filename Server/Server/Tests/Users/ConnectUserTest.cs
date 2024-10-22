using Xunit;
using Server.Model.Managers;
using Server.Model.Data;
using Server.Model.DTO;
using Server.Model;
using Castle.Core.Logging;
using Moq;
using Microsoft.Extensions.Logging;

namespace Tests.Users
{
    /// <summary>
    /// Classe de tests pour la connexion d'un utilisateur.
    /// </summary>
    public class ConnectUserTest
    {
        IUserDAO fakeUserDAO;
        FakeTokenDAO fakeTokenDAO;
        TokenManager tokenManager;
        UserManager userManager;
        Mock<ILogger<UserManager>> mockUserLogger;
        Mock<ILogger<TokenManager>> mockTokenLogger;

        public ConnectUserTest()
        {
            fakeUserDAO = new FakeUserDAO();
            fakeTokenDAO = new FakeTokenDAO(); 
            mockUserLogger = new Mock<ILogger<UserManager>>();
            mockTokenLogger = new Mock<ILogger<TokenManager>>();
            tokenManager = new TokenManager(fakeTokenDAO, mockTokenLogger.Object); 
            userManager = new UserManager(fakeUserDAO, null, tokenManager, mockUserLogger.Object); 
        }

        /// <summary>
        /// Teste la connexion réussie d'un utilisateur avec un bon mot de passe et vérification du token généré.
        /// </summary>
        [Fact]
        public void TestConnectSuccess()
        {

            // Utilisateur pour simuler les informations de connexion
            LoginUserDTO loginUserDto = new LoginUserDTO
            {
                Username = "victor",
                Password = "mdp"
            };

            // Connexion de l'utilisateur via UserManager
            string token = userManager.Connect(loginUserDto);

            // Vérifications
            Assert.NotNull(token);
            Assert.True(fakeTokenDAO.CheckToken("victor", token)); // Vérifie que le token est bien enregistré

        }

        /// <summary>
        /// Teste la tentative de connexion avec un mauvais mot de passe.
        /// </summary>
        [Fact]
        public void TestConnectWrongPassword()
        {
            // Utilisateur avec un mot de passe incorrect
            LoginUserDTO loginUserDto = new LoginUserDTO
            {
                Username = "victor",
                Password = "mauvaismdp"
            };

            // Connexion avec le mauvais mot de passe
            string token = userManager.Connect(loginUserDto);

            // Vérifie que la connexion échoue (aucun token généré)
            Assert.Equal("", token);
        }

        /// <summary>
        /// Teste la tentative de connexion avec un utilisateur inexistant.
        /// </summary>
        [Fact]
        public void TestConnectNonExistentUser()
        {
            // DTO pour un utilisateur qui n'existe pas dans le DAO
            LoginUserDTO loginUserDto = new LoginUserDTO
            {
                Username = "nonExistentUser",
                Password = "password"
            };

            // Tentative de connexion
            string token = userManager.Connect(loginUserDto);

            // Vérifie que la connexion échoue (aucun token généré)
            Assert.Equal("", token);
        }
    }
}
