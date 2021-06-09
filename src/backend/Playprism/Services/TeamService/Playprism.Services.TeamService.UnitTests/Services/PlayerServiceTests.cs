using Moq;
using NUnit.Framework;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using Playprism.Services.TeamService.API.Models;
using Playprism.Services.TeamService.API.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.UnitTests.Services
{
    [TestFixture]
    public class PlayerServiceTests
    {
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private PlayerService _serviceUnderTest;

        private const string username = "testUsername";
        private const string userId = "testUserId";
        private const int id = 123;

        [SetUp]
        public void Setup()
        {
            _playerRepositoryMock = new Mock<IPlayerRepository>();

            _serviceUnderTest = new PlayerService(_playerRepositoryMock.Object);
        }

        [Test]
        public async Task GetPlayerByUserInfoAsync_ShouldAddAndReturnNewPlayer()
        {
            _playerRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PlayerEntity, bool>>>()))
                .ReturnsAsync(new List<PlayerEntity>());
            var userInfo = new UserInfo()
            {
                Username = username,
                UserId = userId
            };
            var expected = new PlayerEntity()
            {
                Name = username,
                UserId = userId,
            };

            var result = await _serviceUnderTest.GetPlayerByUserInfoAsync(userInfo);

            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.UserId, result.UserId);
            _playerRepositoryMock.Verify(x => x.AddAsync(It.Is<PlayerEntity>(p => p.Name == expected.Name && p.UserId == expected.UserId)), Times.Once);
        }

        [Test]
        public async Task GetPlayerByUserInfoAsync_ShouldOnlyReturnPlayer()
        {
            _playerRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PlayerEntity, bool>>>()))
                .Returns(Task.FromResult<IReadOnlyList<PlayerEntity>>(new List<PlayerEntity>() 
                {
                    new PlayerEntity() { Name = username, UserId = userId, Id = id }
                }));
            var userInfo = new UserInfo()
            {
                Username = username,
                UserId = userId
            };
            var expected = new PlayerEntity()
            {
                Name = username,
                UserId = userId,
                Id = id
            };

            var result = await _serviceUnderTest.GetPlayerByUserInfoAsync(userInfo);

            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.UserId, result.UserId);
            _playerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PlayerEntity>()), Times.Never);
        }
    }
}
