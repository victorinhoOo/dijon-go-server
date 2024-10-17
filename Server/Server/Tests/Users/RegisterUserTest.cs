using Xunit;
using Server.Model.Data;
using Server.Model.Managers;
using Server.Model.DTO;
using Moq;
using Server.Model;
using Microsoft.AspNetCore.Http;
using System.Text;
using Server.Model.Images;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Server.Model.Exceptions;

namespace Tests.Users
{

    public class RegisterUserTest
    {
        IUserDAO fakeUserDAO;
        Mock<ImageManager> fakeImageManager;
        Mock<TokenManager> fakeTokenManager;
        Mock<IFileUploader> fakeFileUploader;
        Mock<ILogger<UserManager>> fakeLogger;
        UserManager userManager;

        public RegisterUserTest()
        {
            fakeUserDAO = new FakeUserDAO();
            fakeFileUploader = new Mock<IFileUploader>();
            fakeImageManager = new Mock<ImageManager>(fakeFileUploader.Object);
            fakeTokenManager = new Mock<TokenManager>();
            fakeLogger = new Mock<ILogger<UserManager>>();
            userManager = new UserManager(fakeUserDAO, fakeImageManager.Object, fakeTokenManager.Object, fakeLogger.Object);
        }

        /// <summary>
        /// Teste l'inscription d'un utilisateur avec succès, incluant l'insertion en bdd (fakedao) et l'upload de l'image.
        /// </summary>
        [Fact]
        public void TestRegisterSuccess()
        {
            // créé une fausse image avec du contenu pour pouvoir simuler un upload d'image
            var content = "fake images";
            var fileName = "fakeImage.png";
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            Mock<IFormFile> fakepic = new Mock<IFormFile>();

            // Simule les propriétés de  la fausse image
            fakepic.Setup(f => f.FileName).Returns(fileName);
            fakepic.Setup(f => f.Length).Returns(memoryStream.Length);
            fakepic.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            fakepic.Setup(f => f.ContentDisposition).Returns($"inline; filename={fileName}");

            // création d'un user à inscrire avec les données de test
            RegisterUserDTO registerUserDto = new RegisterUserDTO
            {
                Username = "test",
                Email = "test@test.com",
                Password = "123",
                ProfilePic = fakepic.Object
            };

            // inscription de l'utilisateur via le manager
            userManager.Register(registerUserDto);

            User user = fakeUserDAO.GetUserByUsername("test");

            // vérifie que l'utilisateur a bien été créé
            Assert.NotNull(user);
            Assert.Equal("test@test.com", user.Email);
            Assert.Equal("test", user.Username);

            // vérifie que l'upload de l'image a bien été réalisé
            fakeFileUploader.Verify(uploader => uploader.UploadProfilePic(It.IsAny<IFormFile>(), "test"), Times.Once);
        }

        /// <summary>
        /// Teste l'inscription d'un utilisateur avec un nom d'utilisateur déjà existant.
        /// </summary>
        [Fact]
        public void TestRegisterUsernameAlreadyExists()
        {

            var registerUserDto = new RegisterUserDTO
            {
                Username = "victor",  // ce nom d'utilisateur existe déjà dans le fakedao
                Email = "test@test.com",
                Password = "password",
                ProfilePic = null
            };

            Assert.Throws<UserAlreadyExistsException>(() => userManager.Register(registerUserDto));
        }
    }
}
