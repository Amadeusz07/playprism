using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.UnitTests.DAL.Entities
{
    [TestFixture]
    public class MatchEntityTests
    {
        [Test]
        public void AutoResult_SetWinnerForExistingParticipant1()
        {
            const int expectedMatchResult = 1;
            var entityUnderTest = new MatchEntity()
            {
                Participant1Id = 99,
                Participant2Id = null,
            };

            entityUnderTest.AutoResult();

            Assert.AreEqual(expectedMatchResult, entityUnderTest.Result);
        }
        
        [Test]
        public void AutoResult_SetWinnerForExistingParticipant2()
        {
            const int expectedMatchResult = 2;
            var entityUnderTest = new MatchEntity()
            {
                Participant1Id = null,
                Participant2Id = 99,
            };

            entityUnderTest.AutoResult();
            
            Assert.AreEqual(expectedMatchResult, entityUnderTest.Result);
        }
        
        [Test]
        public void AutoResult_NotSetResults()
        {
            var entityUnderTest = new MatchEntity()
            {
                Participant1Id = 99,
                Participant2Id = 98,
                Result = 2
            };
            var expectedMatchResult = entityUnderTest.Result;

            entityUnderTest.AutoResult();

            Assert.AreEqual(expectedMatchResult, entityUnderTest.Result);
        }
        
        [Test]
        public void AutoResult_SetResultsForNotParticipants()
        {
            const int emptyMatchResult = -1;
            var entityUnderTest = new MatchEntity()
            {
                Participant1Id = null,
                Participant2Id = null,
            };

            entityUnderTest.AutoResult();

            Assert.AreEqual(emptyMatchResult, entityUnderTest.Result);
        }

        [Test]
        public void AutoResult_SetResultsSetMatchStatuses()
        {
            var entityUnderTest = new MatchEntity()
            {
                Played = false,
                Confirmed = false
            };

            entityUnderTest.AutoResult();

            Assert.That(entityUnderTest.Played, Is.True);
            Assert.That(entityUnderTest.Confirmed, Is.True);
        }
    }
}