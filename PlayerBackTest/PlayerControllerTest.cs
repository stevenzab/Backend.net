using Microsoft.AspNetCore.Mvc;
using Moq;
using PlayerBack.Controllers;
using PlayerBack.Models;
using PlayerBack.Services;

namespace PlayerBackTest
{
    [TestClass]
    public sealed class PlayerControllerTest
    {
        [TestClass]
        public class PlayerControllerTests
        {
            private Mock<IPlayerService> _playerServiceMock;
            private PlayerController _controller;

            [TestInitialize]
            public void Setup()
            {
                _playerServiceMock = new Mock<IPlayerService>();
                _controller = new PlayerController(_playerServiceMock.Object);
            }

            [TestMethod]
            public async Task GetPlayerListAsync_ReturnOk()
            {
                //Arrange
                var players = new List<PlayerModel>
            {
                new PlayerModel { Id = "1", FirstName = "Player1" },
                new PlayerModel { Id = "2", FirstName = "Player2" }
            };

                _playerServiceMock
                     .Setup(service => service.GetPlayerListAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(players);


                //Act
                var result = await _controller.GetPlayerListAsync(CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            }
        }
    }
}
