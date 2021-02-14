using Moq;
using NUnit.Framework;
using Playprism.Services.TournamentService.BLL.Services.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.UnitTests.BLL.Services.CompetitionOrganizer
{
    [TestFixture]
    public class SingleEliminationProcessTests
    {
        private Mock<IParticipantRepository> _participantRepositoryMock;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IRoundRepository> _roundRepositoryMock;
        private Mock<IMatchRepository> _matchRepositoryMock;
        private Mock<IRepository<MatchDefinitionEntity>> _matchDefinitionRepositoryMock;

        private SingleEliminationProcess _serviceUnderTest;

        [SetUp]
        public void Setup()
        {
            _participantRepositoryMock = new Mock<IParticipantRepository>();
            _tournamentRepositoryMock = new Mock<ITournamentRepository>();
            _roundRepositoryMock = new Mock<IRoundRepository>();
            _matchRepositoryMock = new Mock<IMatchRepository>();
            _matchDefinitionRepositoryMock = new Mock<IRepository<MatchDefinitionEntity>>();

            _serviceUnderTest = new SingleEliminationProcess(_participantRepositoryMock.Object,
                _tournamentRepositoryMock.Object,
                _roundRepositoryMock.Object,
                _matchRepositoryMock.Object,
                _matchDefinitionRepositoryMock.Object
            );

            // SetupRepositoryData();
        }

        //[Test]
        //public async Task GenerateResponseBracketAsync_ShouldReturnCorrectBracket()
        //{

        //}

        private void SetupRepositoryData()
        {
            _roundRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<RoundEntity, bool>>>(),
                It.IsAny<Func<IQueryable<RoundEntity>, IOrderedQueryable<RoundEntity>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).ReturnsAsync(new List<RoundEntity>()
            {

            });
        }
    }
}