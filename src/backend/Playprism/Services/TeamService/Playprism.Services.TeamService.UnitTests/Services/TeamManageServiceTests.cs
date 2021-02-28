using Moq;
using NUnit.Framework;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interface.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Services;
using Playprism.Services.TeamService.API.Models;
using Playprism.Services.TeamService.API.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.UnitTests.Services
{
    [TestFixture]
    public class TeamManageServiceTests
    {
        private Mock<ITeamRepository> _teamRepositoryMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<IAuth0Repository> _auth0RepositoryMock;
        private Mock<IPlayerService> _playerServiceMock;
        private Mock<ITeamPlayerAssignmentRepository> _teamPlayerAssignmentRepositoryMock;

        private TeamManageService _serviceUnderTest;

        private const string teamName = "TestTestName";
        private const int teamId = 4;
        private const string username = "TestUsername";
        private const int playerId = 5;
        private const string userId = "userid";

        [SetUp]
        public void Setup()
        {
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _auth0RepositoryMock = new Mock<IAuth0Repository>();
            _playerServiceMock = new Mock<IPlayerService>();
            _teamPlayerAssignmentRepositoryMock = new Mock<ITeamPlayerAssignmentRepository>();

            _serviceUnderTest = new TeamManageService(_teamRepositoryMock.Object, 
                _playerRepositoryMock.Object, 
                _auth0RepositoryMock.Object, 
                _playerServiceMock.Object, 
                _teamPlayerAssignmentRepositoryMock.Object
            );
        }

        [Test]
        public async Task AddTeamAsync_ShouldAddTeam()
        {
            var team = new TeamEntity()
            {
                Name = teamName
            };
            _teamRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TeamEntity, bool>>>()))
                .ReturnsAsync(new List<TeamEntity>());

            await _serviceUnderTest.AddTeamAsync(team);

            _teamRepositoryMock.Verify(
                x => x.AddAsync(It.Is<TeamEntity>(t => t.Name == team.Name && t.Active && t.CreateDate != null)), 
                Times.Once
            );
        }

        [Test]
        public void AddTeamAsync_ShouldThrowExceptionWhenDuplicatedNameExists()
        {
            var team = new TeamEntity()
            {
                Name = teamName
            };
            _teamRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TeamEntity, bool>>>()))
                .ReturnsAsync(new List<TeamEntity>()
                {
                    new TeamEntity()
                });

            Assert.ThrowsAsync<ValidationException>(() => _serviceUnderTest.AddTeamAsync(team));
        }

        [Test]
        public async Task InvitePlayerAsync_ShouldAddAssignment()
        {
            var emptyList = new List<TeamPlayerAssignmentEntity>();
            _auth0RepositoryMock.Setup(x => x.SearchUserByNameAsync(username)).ReturnsAsync(new UserInfo()
            {
                UserId = userId
            });
            _playerServiceMock.Setup(x => x.GetPlayerByUserInfoAsync(It.IsAny<UserInfo>())).ReturnsAsync(new PlayerEntity()
            {
                Id = playerId,
                UserId = userId,
                Name = username
            });
            _teamPlayerAssignmentRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<TeamPlayerAssignmentEntity, bool>>>()))
                .ReturnsAsync(emptyList);

            await _serviceUnderTest.InvitePlayerAsync(teamId, username);

            _teamPlayerAssignmentRepositoryMock.Verify(x => x.AddAsync(It.Is<TeamPlayerAssignmentEntity>(t =>
                t.PlayerId == playerId &&
                t.TeamId == teamId &&
                t.InviteDate != null &&
                t.ResponseDate == null &&
                t.LeaveDate == null &&
                t.Accepted == false &&
                t.Active == false
            )), Times.Once);
        }

        [Test]
        public void InvitePlayer_ShouldNotAddAssignmentWhenUserNotFound()
        {
            _auth0RepositoryMock.Setup(x => x.SearchUserByNameAsync(username)).ReturnsAsync(new UserInfo()
            {
                UserId = userId
            });
            _playerServiceMock.Setup(x => x.GetPlayerByUserInfoAsync(It.IsAny<UserInfo>())).ReturnsAsync(new PlayerEntity()
            {
                Id = playerId,
                UserId = userId,
                Name = username
            });
            _teamPlayerAssignmentRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<TeamPlayerAssignmentEntity, bool>>>()))
                .ReturnsAsync(new List<TeamPlayerAssignmentEntity>()
                {
                    new TeamPlayerAssignmentEntity()
                });

            Assert.ThrowsAsync<ValidationException>(() => _serviceUnderTest.InvitePlayerAsync(teamId, username));
        }

        [Test]
        public async Task InvitePlayer_ShouldThrowExceptionWhenNotAllowed()
        {
            _auth0RepositoryMock.Setup(x => x.SearchUserByNameAsync(username))
                .Returns(Task.FromResult<UserInfo>(null));

            await _serviceUnderTest.InvitePlayerAsync(teamId, username);

            _teamPlayerAssignmentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TeamPlayerAssignmentEntity>()), Times.Never);
        }
    }
}
