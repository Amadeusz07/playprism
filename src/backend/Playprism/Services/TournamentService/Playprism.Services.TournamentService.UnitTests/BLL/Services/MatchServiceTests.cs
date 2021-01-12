using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using Playprism.Services.TournamentService.BLL.Services;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.UnitTests.BLL.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IMatchRepository> _mockMatchRepository;
        private Mock<IMapper> _mockMapper;
        private MatchService _serviceUnderTest;
        private MatchEntity correctMatchResult;
        private MatchEntity correctDatabaseMatchEntity;

        [SetUp]
        public void Setup()
        {
            _mockMatchRepository = new Mock<IMatchRepository>();
            _mockMapper = new Mock<IMapper>();
            _serviceUnderTest = new MatchService(_mockMatchRepository.Object, _mockMapper.Object);
            
            SetupRepositoryData();
        }
        
        [Test]
        public async Task SetResultAsync_ShouldUpdateMatchResult()
        {
            var matchResult = new MatchEntity
            {
                Id = 99,
                MatchDate = null,
                Participant1Score = 3,
                Participant2Score = 2,
                Result = 1,
                Played = false,
                Confirmed = false
            };

            var result = await _serviceUnderTest.SetResultAsync(matchResult);
            
            Assert.IsTrue(result.Played);
            Assert.IsTrue(result.Confirmed);
        }

        private void SetupRepositoryData()
        {
            // _mockMatchRepository.Setup(x => x.GetByIdAsync())
        }
    }
}