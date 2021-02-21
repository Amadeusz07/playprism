using AutoMapper;
using Moq;
using NUnit.Framework;
using Playprism.Services.TournamentService.BLL.Services;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.UnitTests.BLL.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IMatchRepository> _mockMatchRepository;
        private Mock<IMapper> _mockMapper;
        private MatchService _serviceUnderTest;

        private int matchId = 99;

        [SetUp]
        public void Setup()
        {
            _mockMatchRepository = new Mock<IMatchRepository>();
            _mockMapper = new Mock<IMapper>();
            _serviceUnderTest = new MatchService(_mockMatchRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task ConfirmMatchAsync_ShouldConfirmMatchResult()
        {
            SetupRepositoryData(new MatchEntity()
            {
                Participant1Score = 3,
                Participant2Score = 4,
                Confirmed = false,
                Result = 2
            });

            var result = await _serviceUnderTest.ConfirmMatchAsync(matchId);
            
            Assert.IsTrue(result.Confirmed);
        }
        
        [Test]
        public async Task SetResultAsync_ShouldUpdateMatchResultAndConfirm()
        {
            var matchResult = new MatchEntity
            {
                Id = matchId,
                MatchDate = null,
                Participant1Score = 3,
                Participant2Score = 2,
                Result = 1,
                Played = false,
                Confirmed = false
            };
            SetupRepositoryData(new MatchDefinitionEntity()
            {
                ScoreBased = false,
                NumberOfGames = 5,
                ConfirmationNeeded = false
            });

            var result = await _serviceUnderTest.SetResultAsync(matchResult);
            
            Assert.IsTrue(result.Played);
            Assert.IsTrue(result.Confirmed);
            Assert.AreEqual(1, result.Result);
            Assert.AreEqual(3, result.Participant1Score);
            Assert.AreEqual(2, result.Participant2Score);
        }

        [Test]
        public async Task SetResultAsync_ShouldUpdateMatchResultAndNotConfirm()
        {
            var matchResult = new MatchEntity
            {
                Id = matchId,
                MatchDate = null,
                Participant1Score = 3,
                Participant2Score = 2,
                Result = 1,
                Played = false,
                Confirmed = false
            };            
            SetupRepositoryData(new MatchDefinitionEntity()
            {
                ScoreBased = false,
                NumberOfGames = 5,
                ConfirmationNeeded = true
            });
            
            var result = await _serviceUnderTest.SetResultAsync(matchResult);
            
            Assert.IsTrue(result.Played);
            Assert.IsFalse(result.Confirmed);
            Assert.AreEqual(1, result.Result);
            Assert.AreEqual(3, result.Participant1Score);
            Assert.AreEqual(2, result.Participant2Score);
        }

        [Test]
        public void SetResultAsync_ThrowsExceptionWhenScoreIsLessThanZero()
        {
            var matchResult = new MatchEntity
            {
                Id = matchId,
                Participant1Score = -1,
                Participant2Score = 2,
            };
            SetupRepositoryData(new MatchDefinitionEntity());
            
            Assert.ThrowsAsync<ValidationException>(
                ()  => _serviceUnderTest.SetResultAsync(matchResult),
                $"One of scores of match {matchResult.Id} is less that 0");
        }
        
        [Test]
        public void SetResultAsync_NotAllowDrawForNonScoreBased()
        {
            var matchResult = new MatchEntity
            {
                Id = matchId,
                Participant1Score = 4,
                Participant2Score = 0,
                Result = 0
            };
            SetupRepositoryData(new MatchDefinitionEntity()
            {
                ScoreBased = false,
                NumberOfGames = 7
            });
            
            Assert.ThrowsAsync<ValidationException>(
                ()  => _serviceUnderTest.SetResultAsync(matchResult),
                $"Result of match {matchResult.Id} cannot be a draw in ScoreBased set to false");
        }

        private void SetupRepositoryData(MatchDefinitionEntity matchDefinitionEntity)
        {
            _mockMatchRepository.Setup(x => x.GetFullByIdAsync(matchId))
                .ReturnsAsync(new MatchEntity()
                {
                    Id = matchId,
                    Played = false,
                    Result = null,
                    Participant1Score = null,
                    Participant2Score = null,
                    Confirmed = false,
                    Round = new RoundEntity()
                    {
                        MatchDefinition = matchDefinitionEntity
                    }
                });
        }
        
        private void SetupRepositoryData(MatchEntity entity)
        {
            _mockMatchRepository.Setup(x => x.GetByIdAsync(matchId))
                .ReturnsAsync(entity);
        }
    }
}