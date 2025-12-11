using Microsoft.AspNetCore.Mvc;
using Moq;
using PlayerBack.Controllers;
using PlayerBack.Models;
using PlayerBack.Services;

namespace PlayerBackTest.ControllerTest
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
            [TestMethod]
            public async Task GetPlayerByIdAsync_ReturnsNotFound_WhenPlayerDoesNotExist()
            {
                // Arrange
                _playerServiceMock
                    .Setup(s => s.GetByIdAsync("missing", It.IsAny<CancellationToken>()))
                    .ReturnsAsync((PlayerModel?)null);

                // Act
                var result = await _controller.GetPlayerByIdAsync("missing", CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task GetPlayerStatisticsAsync_ReturnsOk_WithStatistics()
            {
                // Arrange
                var stats = new StatisticsModel
                {
                    CountryCodeWithHighestWinRatio = "USA",
                    HighestWinRatio = 0.75,
                    AverageBmi = 23.5,
                    MedianHeight = 178
                };

                _playerServiceMock
                    .Setup(s => s.GetStatisticsAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(stats);

                // Act
                var result = await _controller.GetPlayerStatisticsAsync(CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
                var ok = result.Result as OkObjectResult;
                Assert.IsNotNull(ok);
                Assert.AreSame(stats, ok.Value);
            }

            [TestMethod]
            public async Task GetPlayerStatisticsAsync_ReturnsNotFound_WhenNoPlayers()
            {
                // Arrange
                _playerServiceMock
                    .Setup(s => s.GetStatisticsAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync((StatisticsModel?)null);

                // Act
                var result = await _controller.GetPlayerStatisticsAsync(CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task CreatePlayerAsync_ReturnsCreatedAtRoute()
            {
                // Arrange
                var player = new PlayerModel
                {
                    Id = "42",
                    FirstName = "New",
                    LastName = "Player",
                    Country = new CountryModel { Code = "ESP", Picture = string.Empty },
                    Data = new DataModel { Height = 170, Weight = 68, Last = new List<int>() }
                };

                _playerServiceMock
                    .Setup(s => s.CreateAsync(player, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.CreatePlayerAsync(player, CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
                var created = result as CreatedAtRouteResult;
                Assert.IsNotNull(created);
                Assert.AreEqual(player, created.Value);
            }

            [TestMethod]
            public async Task DeletePlayerByIdAsync_ReturnsNoContent_WhenDeleted()
            {
                // Arrange
                _playerServiceMock
                    .Setup(s => s.DeleteByIdAsync("1", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

                // Act
                var result = await _controller.DeletePlayerByIdAsync("1", CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }

            [TestMethod]
            public async Task DeletePlayerByIdAsync_ReturnsNotFound_WhenNotDeleted()
            {
                // Arrange
                _playerServiceMock
                    .Setup(s => s.DeleteByIdAsync("1", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(false);

                // Act
                var result = await _controller.DeletePlayerByIdAsync("1", CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task DeleteAllPlayerAsync_ReturnsNoContent()
            {
                // Arrange
                _playerServiceMock
                    .Setup(s => s.DeleteAllAsync(It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.DeleteAllPlayerAsync(CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }
    }
}
