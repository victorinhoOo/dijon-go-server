using Xunit;
using Moq;
using Server.Model.Managers;
using Server.Model.Data;
using Server.Model.DTO;
using Server.Model;
using System.Text;
using Microsoft.AspNetCore.Http;
using Server.Model.Images;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Tests.Users
{
    public class UpdateUserTest
    {
        IUserDAO fakeUserDAO;
        ITokenDAO fakeTokenDAO;
        Mock<ImageManager> fakeImageManager;
        Mock<IFileUploader> fakeFileUploader;
        Mock<ILogger<UserManager>> mockUserLogger;
        Mock<ILogger<TokenManager>> mockTokenLogger;
        TokenManager tokenManager;
        UserManager userManager;

        public UpdateUserTest()
        {
            fakeTokenDAO = new FakeTokenDAO();
            fakeUserDAO = new FakeUserDAO();
            fakeFileUploader = new Mock<IFileUploader>();
            fakeImageManager = new Mock<ImageManager>(fakeFileUploader.Object);
            mockUserLogger = new Mock<ILogger<UserManager>>();
            mockTokenLogger = new Mock<ILogger<TokenManager>>();
            tokenManager = new TokenManager(fakeTokenDAO, mockTokenLogger.Object);
            userManager = new UserManager(fakeUserDAO, fakeImageManager.Object, tokenManager, mockUserLogger.Object);
        }

        /// <summary>
        /// Teste la mise à jour réussie d'un utilisateur avec un nouveau nom d'utilisateur, un nouveau mot de passe et un nouveau mail.
        /// </summary>
        [Fact]
        public void TestUpdateUserSuccess()
        {

            // DTO pour simuler les nouvelles informations de l'utilisateur
            UpdateUserDTO updateUserDto = new UpdateUserDTO
            {
                Tokenuser = "abc123",
                Username = "VictorNew",
                Oldpassword = "mdp",
                Password = "MdpNew",
                Email = "nouvelemail@gmail.com"
            };

            // Mise à jour de l'utilisateur via UserManager
            userManager.UpdateUser(updateUserDto);


            // Vérifie que l'utilisateur a été mis à jour dans le DAO
            User updatedUser = fakeUserDAO.GetUserByUsername("VictorNew");
            Assert.NotNull(updatedUser);
            Assert.Equal(updateUserDto.Username, updatedUser.Username);
            Assert.Equal(userManager.HashPassword("MdpNew"), updatedUser.Password);
            Assert.Equal(updateUserDto.Email, updatedUser.Email);
        }

        /// <summary>
        /// Teste la tentative de mise à jour d'un utilisateur avec le mauvais mot de passe.
        /// </summary>
        [Fact]
        public void TestUpdateWrongMdp()
        {
            UpdateUserDTO updateUserDto = new UpdateUserDTO
            {
                Oldpassword = "mauvaismdp",
                Tokenuser = "abc123",
                Username = "VictorNewww"
            };

            // Vérifie que la mise à jour échoue et que l'exception UnauthorizedAccessException est levée
            Assert.Throws<UnauthorizedAccessException>(() => userManager.UpdateUser(updateUserDto));
        }

        // <summary>
        /// Teste la mise à jour de la photo de profil d'un utilisateur.
        /// </summary>
        [Fact]
        public void TestUpdateProfilePicSuccess()
        {
            // Création d'une fausse image pour simuler l'upload d'une nouvelle photo de profil
            var content = "fake image content";
            var fileName = "newProfilePic.png";
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            Mock<IFormFile> fakeProfilePic = new Mock<IFormFile>();

            // Simule les propriétés de la fausse image
            fakeProfilePic.Setup(f => f.FileName).Returns(fileName);
            fakeProfilePic.Setup(f => f.Length).Returns(memoryStream.Length);
            fakeProfilePic.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            fakeProfilePic.Setup(f => f.ContentDisposition).Returns($"inline; filename={fileName}");


            UpdateUserDTO updateUserDto = new UpdateUserDTO
            {
                Oldpassword = "mdp",
                Tokenuser = "abc123",
                ProfilePic = fakeProfilePic.Object
            };

            // update de la photo de profil 
            userManager.UpdateUser(updateUserDto);

            // Vérifie que l'upload a réussit
            fakeFileUploader.Verify(uploader => uploader.UploadProfilePic(It.IsAny<IFormFile>(), "victor"), Times.Once);
        }
    }
}
