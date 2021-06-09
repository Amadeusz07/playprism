using FluentAssertions;
using Moq;
using NUnit.Framework;
using Playprism.Services.TournamentService.BLL.Services;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.UnitTests.BLL.Services
{
    [TestFixture]
    public class PlayerStatisticsTests
    {
        private Mock<IParticipantRepository> _participantRepositoryMock;
        private Mock<IMatchRepository> _matchRepositoryMock;
        private Mock<IRepository<DisciplineEntity>> _disciplineRepositoryMock;

        private PlayerStatisticsService _serviceUnderTest;

        private readonly string userId = "userId";

        [SetUp]
        public void Setup()
        {
            _matchRepositoryMock = new Mock<IMatchRepository>();
            _participantRepositoryMock = new Mock<IParticipantRepository>();
            _disciplineRepositoryMock = new Mock<IRepository<DisciplineEntity>>();

            _serviceUnderTest = new PlayerStatisticsService(_participantRepositoryMock.Object, _matchRepositoryMock.Object, _disciplineRepositoryMock.Object);
        }

        //[Test]
        //public async Task GetPlayerRecords_ReturnsCorrectData()
        //{
        //    var result = await _serviceUnderTest.GetPlayerRecords(userId);

        //    result.Should().NotBeEmpty();
        //}

        public void SetupParticipantData()
        {
            _participantRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<ParticipantEntity, bool>>>())
            ).ReturnsAsync(new List<ParticipantEntity> {
                new ParticipantEntity { Id = 99, Tournament = new TournamentEntity { DisciplineId = 1 } },
                new ParticipantEntity { Id = 98, Tournament = new TournamentEntity { DisciplineId = 1 } },
                new ParticipantEntity { Id = 97, Tournament = new TournamentEntity { DisciplineId = 2 } }
            });
        }

        public void SetupDisciplineData()
        {
            _disciplineRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new DisciplineEntity { Id = 1, Name = "Discipline1" });
            _disciplineRepositoryMock.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new DisciplineEntity { Id = 2, Name = "Discipline2" });
        }
    }
}
