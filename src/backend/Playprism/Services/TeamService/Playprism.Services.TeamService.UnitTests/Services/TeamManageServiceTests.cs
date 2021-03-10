using AutoMapper;
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
using System.Linq;
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
        private Mock<IMapper> _mapperMock;

        private TeamManageService _serviceUnderTest;

        private const string teamName = "TestTestName";
        private const int teamId = 4;
        private const int teamId2 = 5;
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
            _mapperMock = new Mock<IMapper>();

            _serviceUnderTest = new TeamManageService(_teamRepositoryMock.Object, 
                _playerRepositoryMock.Object, 
                _auth0RepositoryMock.Object, 
                _playerServiceMock.Object, 
                _teamPlayerAssignmentRepositoryMock.Object,
                _mapperMock.Object
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
        public void InvitePlayerAsync_ShouldNotAddAssignmentWhenUserNotFound()
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
        public async Task InvitePlayerAsync_ShouldThrowExceptionWhenNotAllowed()
        {
            _auth0RepositoryMock.Setup(x => x.SearchUserByNameAsync(username))
                .Returns(Task.FromResult<UserInfo>(null));

            await _serviceUnderTest.InvitePlayerAsync(teamId, username);

            _teamPlayerAssignmentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TeamPlayerAssignmentEntity>()), Times.Never);
        }

        [Test]
        public async Task JoinTeamAsync_ShouldUpdateAssignment()
        {
            var assignments = new List<TeamPlayerAssignmentEntity>()
            {
                new TeamPlayerAssignmentEntity()
                {
                    PlayerId = playerId,
                    TeamId = teamId,
                    Accepted = false,
                    Active = false,
                },
                new TeamPlayerAssignmentEntity()
                {
                    PlayerId = playerId,
                    TeamId = teamId2,
                    LeaveDate = DateTime.Now,
                    Accepted = true,
                    Active = false,
                }
            };
            SetupPlayerRepositoryMockGetAsync(assignments);

            await _serviceUnderTest.JoinTeamAsync(userId, teamId);

            _teamPlayerAssignmentRepositoryMock.Verify(x => x.UpdateAsync(It.Is<TeamPlayerAssignmentEntity>(a =>
                a.TeamId == teamId &&
                a.PlayerId == playerId &&
                a.Accepted == true &&
                a.Active == true &&
                a.ResponseDate != null &&
                a.LeaveDate == null
            )), Times.Once);
        }

        [Test]
        public void JoinTeamAsync_ShouldNotAcceptWhenAlreadyHasTeam()
        {
            var assignments = new List<TeamPlayerAssignmentEntity>()
            {
                new TeamPlayerAssignmentEntity()
                {
                    PlayerId = playerId,
                    TeamId = teamId,
                    Accepted = false,
                    Active = false,
                },
                new TeamPlayerAssignmentEntity()
                {
                    PlayerId = playerId,
                    TeamId = teamId2,
                    Active = true,
                }
            };
            SetupPlayerRepositoryMockGetAsync(assignments);

            Assert.ThrowsAsync<ValidationException>(() => _serviceUnderTest.JoinTeamAsync(userId, teamId));
        }

        [Test]
        public async Task LeaveTeamAsync_ShouldUpdateAssignment()
        {
            var assignments = new List<TeamPlayerAssignmentEntity>()
            {
                new TeamPlayerAssignmentEntity()
                {
                    PlayerId = playerId,
                    TeamId = teamId,
                    Accepted = true,
                    Active = true,
                    InviteDate = DateTime.Now,
                    ResponseDate = DateTime.Now.AddDays(1),
                    LeaveDate = null
                }
            };
            SetupPlayerRepositoryMockGetAsync(assignments);

            await _serviceUnderTest.LeaveTeamAsync(userId, teamId);

            _teamPlayerAssignmentRepositoryMock.Verify(x => x.UpdateAsync(It.Is<TeamPlayerAssignmentEntity>(a =>
                a.TeamId == teamId &&
                a.PlayerId == playerId &&
                a.Accepted == true &&
                a.Active == false &&
                a.ResponseDate != null &&
                a.InviteDate != null &&
                a.LeaveDate != null
            )), Times.Once);
        }

        [Test]
        public async Task RefuseTeamAsync_ShouldUpdateAssignment()
        {
            var assignments = new List<TeamPlayerAssignmentEntity>()
            {
                new TeamPlayerAssignmentEntity()
                {
                    PlayerId = playerId,
                    TeamId = teamId,
                    Accepted = false,
                    Active = false,
                    InviteDate = DateTime.Now,
                }
            };
            SetupPlayerRepositoryMockGetAsync(assignments);

            await _serviceUnderTest.RefuseTeamAsync(userId, teamId);

            _teamPlayerAssignmentRepositoryMock.Verify(x => x.UpdateAsync(It.Is<TeamPlayerAssignmentEntity>(a =>
                a.TeamId == teamId &&
                a.PlayerId == playerId &&
                a.Accepted == false &&
                a.Active == false &&
                a.ResponseDate != null &&
                a.LeaveDate == null
            )), Times.Once);
        }

        private void SetupPlayerRepositoryMockGetAsync(List<TeamPlayerAssignmentEntity> toReturn)
        {
            _playerRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<PlayerEntity, bool>>>(),
                It.IsAny<Func<IQueryable<PlayerEntity>, IOrderedQueryable<PlayerEntity>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).ReturnsAsync(new List<PlayerEntity>
                {
                    new PlayerEntity()
                    {
                        Id = playerId,
                        UserId = userId,
                        Assignments = toReturn
                    }
                });
        }
    }
}
