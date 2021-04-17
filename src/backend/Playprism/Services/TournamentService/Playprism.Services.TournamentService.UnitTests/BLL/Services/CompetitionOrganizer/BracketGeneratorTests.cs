using Moq;
using NUnit.Framework;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.BLL.Services.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

namespace Playprism.Services.TournamentService.UnitTests.BLL.Services.CompetitionOrganizer
{
    [TestFixture]
    public class BracketGeneratorTests
    {
        private Mock<IRoundRepository> _roundRepositoryMock;
        private Mock<IMatchRepository> _matchRepositoryMock;
        private Mock<IRepository<MatchDefinitionEntity>> _matchDefinitiorRepositoryMock;

        private IBracketGenerator _serviceUnderTest;

        private int matchDefinitionId = 99;
        private int tournamentId = 9;

        [SetUp]
        public void Setup()
        {
            _roundRepositoryMock = new Mock<IRoundRepository>();
            _matchRepositoryMock = new Mock<IMatchRepository>();
            _matchDefinitiorRepositoryMock = new Mock<IRepository<MatchDefinitionEntity>>();

            _serviceUnderTest = new BracketGenerator(
                _roundRepositoryMock.Object,
                _matchRepositoryMock.Object,
                _matchDefinitiorRepositoryMock.Object
            );
        }

        [Test]
        [TestCase(2, 2)]
        [TestCase(4, 2)]
        [TestCase(8, 4)]
        [TestCase(8, 5)]
        public async Task GenerateRoundsAsync_SetDefaultMatchDefinition(int maxNumberOfPlayer, int participantCount)
        {
            SetupMatchDefinitionRepository();
            var tournament = new TournamentEntity
            {
                Id = tournamentId,
                MaxNumberOfPlayers = maxNumberOfPlayer
            };
            int expectedMatchDefinitionId = 99;

            var actual = await _serviceUnderTest.GenerateRoundsAsync(tournament, participantCount);

            actual.Should().OnlyContain(x => x.MatchDefinitionId == expectedMatchDefinitionId);
        }

        [Test]
        [TestCase(2, 2, 1)]
        [TestCase(4, 2, 1)]
        [TestCase(4, 3, 2)]
        [TestCase(8, 3, 2)]
        [TestCase(8, 5, 3)]
        [TestCase(16, 2, 1)]
        public async Task GenerateRoundsAsync_ReturnsCorrectRounds(
            int maxNumberOfPlayers, 
            int participantCount, 
            int expectedNumberOfRounds)
        {
            SetupMatchDefinitionRepository();
            var tournament = new TournamentEntity
            {
                Id = tournamentId,
                MaxNumberOfPlayers = maxNumberOfPlayers
            };

            var actual = await _serviceUnderTest.GenerateRoundsAsync(tournament, participantCount);

            actual.Should().HaveCount(expectedNumberOfRounds);
            actual.Should().OnlyContain(x => x.Started == false && x.Finished == false);
            actual.Should().BeInAscendingOrder(x => x.Order);
            _roundRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RoundEntity>()), Times.Exactly(expectedNumberOfRounds));
        }

        [Test]
        public async Task GenerateRoundsAsync_ReturnsRoundWithCorrectNumbers_For8OutOf8()
        {
            var participantCount = 8;
            var maxNumberOfPlayers = 8;
            SetupMatchDefinitionRepository();
            var tournament = new TournamentEntity
            {
                Id = tournamentId,
                MaxNumberOfPlayers = maxNumberOfPlayers
            };
            var expected = CreateThreeRounds(null);

            var actual = await _serviceUnderTest.GenerateRoundsAsync(tournament, participantCount);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GenerateRoundsAsync_ReturnsRoundWithCorrectNumbers_For2OutOf16()
        {
            var participantCount = 2;
            var maxNumberOfPlayers = 16;
            SetupMatchDefinitionRepository();
            var tournament = new TournamentEntity
            {
                Id = tournamentId,
                MaxNumberOfPlayers = maxNumberOfPlayers
            };
            var expected = new List<RoundEntity>()
            {
                new RoundEntity()
                {
                    TournamentId = tournamentId,
                    MatchDefinitionId = matchDefinitionId,
                    Order = 0,
                    StartDate = null,
                    Finished = false,
                    Started = false,
                    Matches = new List<MatchEntity>(),
                    NumberOfCompetitors = 2,
                    NumberOfMatches = 1
                }
            };

            var actual = await _serviceUnderTest.GenerateRoundsAsync(tournament, participantCount);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GenerateMatchesAsync_ReturnsCorrectNumberOfMatches()
        {
            var defaultMatchDefinition = SetupMatchDefinitionRepository(false);
            var rounds = CreateThreeRounds(defaultMatchDefinition);
            rounds[0].Id = 1;
            rounds[1].Id = 2;
            rounds[2].Id = 5;
            var expectedNumberOfMatches = 7;
            var expectedNumberOfMatchesInRound1 = 4;
            var expectedNumberOfMatchesInRound2 = 2;
            var expectedNumberOfMatchesInRound5 = 1;
            var expectedNumberOfRoundUpdateCalls = 3;

            var actual = await _serviceUnderTest.GenerateMatchesAsync(rounds);

            actual.Should().HaveCount(expectedNumberOfMatches);
            actual.Where(x => x.RoundId == 1).Should().HaveCount(expectedNumberOfMatchesInRound1);
            actual.Where(x => x.RoundId == 2).Should().HaveCount(expectedNumberOfMatchesInRound2);
            actual.Where(x => x.RoundId == 5).Should().HaveCount(expectedNumberOfMatchesInRound5);
            _roundRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<RoundEntity>()), Times.Exactly(expectedNumberOfRoundUpdateCalls));
        }

        private MatchDefinitionEntity SetupMatchDefinitionRepository(bool confirmationNeeded = false)
        {
            var matchDefinition = new MatchDefinitionEntity
            {
                Id = matchDefinitionId,
                Name = "Default",
                ConfirmationNeeded = confirmationNeeded,
            };
            _matchDefinitiorRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<MatchDefinitionEntity, bool>>>()))
                .ReturnsAsync(new List<MatchDefinitionEntity>()
                {
                    matchDefinition
                });

            return matchDefinition;
        }

        private List<RoundEntity> CreateThreeRounds(MatchDefinitionEntity matchDefinition)
        {
            return new List<RoundEntity>()
            {
                new RoundEntity()
                {
                    TournamentId = tournamentId,
                    MatchDefinitionId = matchDefinitionId,
                    MatchDefinition = matchDefinition,
                    Order = 0,
                    StartDate = null,
                    Finished = false,
                    Started = false,
                    Matches = new List<MatchEntity>(),
                    NumberOfCompetitors = 8,
                    NumberOfMatches = 4
                },
                new RoundEntity()
                {
                    TournamentId = tournamentId,
                    MatchDefinitionId = matchDefinitionId,
                    MatchDefinition = matchDefinition,
                    Order = 1,
                    StartDate = null,
                    Finished = false,
                    Started = false,
                    Matches = new List<MatchEntity>(),
                    NumberOfCompetitors = 4,
                    NumberOfMatches = 2
                },
                new RoundEntity()
                {
                    TournamentId = tournamentId,
                    MatchDefinitionId = matchDefinitionId,
                    MatchDefinition = matchDefinition,
                    Order = 2,
                    StartDate = null,
                    Finished = false,
                    Started = false,
                    Matches = new List<MatchEntity>(),
                    NumberOfCompetitors = 2,
                    NumberOfMatches = 1
                },
            };
        }
    }
}
