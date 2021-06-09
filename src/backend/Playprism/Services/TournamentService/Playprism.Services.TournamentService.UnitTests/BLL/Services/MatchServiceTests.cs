using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Mappings;
using Playprism.Services.TournamentService.BLL.Services;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.UnitTests.BLL.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IMatchRepository> _mockMatchRepository;
        private Mock<IParticipantRepository> _mockParticipantRepository;
        private IMapper _mockMapper;
        private MatchService _serviceUnderTest;

        private readonly int matchId = 99;
        private readonly string userId = "userId";

        [SetUp]
        public void Setup()
        {
            _mockMatchRepository = new Mock<IMatchRepository>();
            _mockParticipantRepository = new Mock<IParticipantRepository>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EntitiesMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mockMapper = mapper;
            _serviceUnderTest = new MatchService(_mockMatchRepository.Object, _mockParticipantRepository.Object, _mockMapper);
        }

        [Test]
        public async Task GetIncomingMatchListAsync_ReturnsFormattedMatches()
        {
            _mockParticipantRepository.Setup(x =>
                x.GetParticipantsByUserIdAsync(userId)
            ).ReturnsAsync(new List<ParticipantEntity>()
            {
                new ParticipantEntity() { Id = 1, UserId = userId },
                new ParticipantEntity() { Id = 2, UserId = userId }
            });
            var participantIdsInMatches = new List<int> { 1, 6, 2, 11, 5, 10 };
            _mockParticipantRepository.Setup(x =>
                x.GetByIdsAsync(participantIdsInMatches)
            ).ReturnsAsync(new List<ParticipantEntity>()
            { 
                new ParticipantEntity() { Id = 1, Name = "Test1" },
                new ParticipantEntity() { Id = 5, Name = "Test5" },
                new ParticipantEntity() { Id = 6, Name = "Test6" },
                new ParticipantEntity() { Id = 2, Name = "Test2" },
                new ParticipantEntity() { Id = 10, Name = "Test10" },
                new ParticipantEntity() { Id = 11, Name = "Test11" }
            });
            _mockMatchRepository.Setup(x =>
                x.GetIncomingMatchesAsync(1)
            ).ReturnsAsync(new List<MatchEntity>()
            {
                new MatchEntity() { Id = 99, Participant1Id = 1, Participant2Id = 5, MatchDate = new DateTime(2020, 11, 11) },
                new MatchEntity() { Id = 98, Participant1Id = 6, Participant2Id = 1, MatchDate = new DateTime(2020, 10, 11) }
            });
            _mockMatchRepository.Setup(x =>
                x.GetIncomingMatchesAsync(2)
            ).ReturnsAsync(new List<MatchEntity>()
            {
                new MatchEntity() { Id = 97, Participant1Id = 2, Participant2Id = 10, MatchDate = new DateTime(2020, 12, 11) },
                new MatchEntity() { Id = 96, Participant1Id = 11, Participant2Id = 2, MatchDate = new DateTime(2021, 1, 11) }
            });
            var expected = new List<MatchDto>
            {
                new MatchDto() { Id = 98, Participant1Id = 6, Participant2Id = 1, MatchDate = new DateTime(2020, 10, 11), Participant1Name = "Test6", Participant2Name = "Test1" },
                new MatchDto() { Id = 99, Participant1Id = 1, Participant2Id = 5, MatchDate = new DateTime(2020, 11, 11), Participant1Name = "Test1", Participant2Name = "Test5" },
                new MatchDto() { Id = 97, Participant1Id = 2, Participant2Id = 10, MatchDate = new DateTime(2020, 12, 11), Participant1Name = "Test2", Participant2Name = "Test10" },
                new MatchDto() { Id = 96, Participant1Id = 11, Participant2Id = 2, MatchDate = new DateTime(2021, 1, 11), Participant1Name = "Test11", Participant2Name = "Test2" }
            };

            var actual = await _serviceUnderTest.GetIncomingMatchListAsync(userId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetIncomingMatchListAsync_ReturnsMatchesForNullParticipant()
        {
            _mockParticipantRepository.Setup(x =>
                x.GetParticipantsByUserIdAsync(userId)
            ).ReturnsAsync(new List<ParticipantEntity>()
            {
                new ParticipantEntity() { Id = 1, UserId = userId }
            });
            var participantIdsInMatches = new List<int> { 1 };
            _mockParticipantRepository.Setup(x =>
                x.GetByIdsAsync(participantIdsInMatches)
            ).ReturnsAsync(new List<ParticipantEntity>()
            {
                new ParticipantEntity() { Id = 1, Name = "Test1" }
            });
            _mockMatchRepository.Setup(x =>
                x.GetIncomingMatchesAsync(1)
            ).ReturnsAsync(new List<MatchEntity>()
            {
                new MatchEntity() { Id = 99, Participant1Id = 1, Participant2Id = null, MatchDate = new DateTime(2020, 11, 11) }
            });
            var expected = new List<MatchDto>
            {
                new MatchDto() { Id = 99, Participant1Id = 1, Participant2Id = null, MatchDate = new DateTime(2020, 11, 11), Participant1Name = "Test1", Participant2Name = "EMPTY" }
            };

            var actual = await _serviceUnderTest.GetIncomingMatchListAsync(userId);

            actual.Should().BeEquivalentTo(expected);
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