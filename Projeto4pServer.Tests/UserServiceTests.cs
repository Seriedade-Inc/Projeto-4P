using System.Threading.Tasks;
using Xunit;
using Moq;
using Projeto4pServer.Services;
using Projeto4pServer.Repository;
using Projeto4pSharedLibrary.Classes;
using Projeto4pServer.DTOs;
using Microsoft.AspNetCore.Http;

namespace Projeto4pServer.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task RegisterUserAsync_ReturnsError_WhenEmailExists()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.EmailExistsAsync(It.Is<string>(email => email == "teste@teste"))).ReturnsAsync(true);
            userRepoMock.Setup(r => r.EmailExistsAsync(It.Is<string>(email => email != "teste@teste"))).ReturnsAsync(false);
            var httpContextMock = new Mock<IHttpContextAccessor>();
            var service = new UserService(userRepoMock.Object, httpContextMock.Object);
            var dto = new UserRegisterDto { Email = "teste@teste", UserName = "TestandoUserName", Password = "123" };

            // Act
            var result = await service.RegisterUserAsync(dto);

            // Assert
            Assert.Equal("Email já está em uso.", result);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsError_WhenEmailOrUsernameExists()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.EmailExistsAsync("teste@teste")).ReturnsAsync(true);
            userRepoMock.Setup(r => r.UsernameExistsAsync("teste")).ReturnsAsync(true);
            var httpContextMock = new Mock<IHttpContextAccessor>();
            var service = new UserService(userRepoMock.Object, httpContextMock.Object);
            var dto = new UserRegisterDto { Email = "teste@teste", UserName = "teste", Password = "123" };

            // Act
            var result = await service.RegisterUserAsync(dto);

            // Assert
            Assert.True(result == "Email já está em uso." || result == "Este nome já está em uso.");
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsError_WhenUsernameExists()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            userRepoMock.Setup(r => r.UsernameExistsAsync("teste")).ReturnsAsync(true);
            var httpContextMock = new Mock<IHttpContextAccessor>();
            var service = new UserService(userRepoMock.Object, httpContextMock.Object);
            var dto = new UserRegisterDto { Email = "novo@teste.com", UserName = "teste", Password = "123" };

            // Act
            var result = await service.RegisterUserAsync(dto);

            // Assert
            Assert.Equal("Este nome já está em uso.", result);
        }
    }
}