using System;
using System.Collections.Generic;
using NUnit.Framework;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.UnitTests.DAL.Entities
{
    [TestFixture]
    public class RoundEntityTests
    {
        [Test]
        public void GetWinnerIds_ReturnCorrectWinners()
        {
            var participant1WinnerResult = 1;
            var participant2WinnerResult = 2;
            var drawResult = 0;
            var emptyAutoResult = -1;
            var roundEntity = new RoundEntity()
            {
                Finished = true,
                Matches = new List<MatchEntity>()
                {
                    new MatchEntity() { Participant1Id = 99, Participant2Id = 98, Result = participant1WinnerResult, Played = true, Confirmed = true },
                    new MatchEntity() { Participant1Id = 97, Participant2Id = 96, Result = participant2WinnerResult, Played = true, Confirmed = true },
                    new MatchEntity() { Participant1Id = 95, Participant2Id = 94, Result = drawResult, Played = true, Confirmed = true },
                    new MatchEntity() { Participant1Id = null, Participant2Id = null, Result = emptyAutoResult, Played = true, Confirmed = true }
                }
            };
            var expectedResult = new List<int> { 99, 96 };

            var result = roundEntity.GetWinnerIds();
            
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void GetWinnerIds_ThrowsExceptionWhenNotFinished()
        {
            var roundEntity = new RoundEntity()
            {
                Finished = false
            };

            Assert.Throws<InvalidOperationException>(() => roundEntity.GetWinnerIds());
        }

        [Test]
        public void GetWinnerIds_ThrowsExceptionWhenAtLeastOneMatchIsNotPlayed()
        {
            var roundEntity = new RoundEntity()
            {
                Finished = true,
                Matches = new List<MatchEntity>()
                {
                    new MatchEntity() { Played = true, Confirmed = true },
                    new MatchEntity() { Played = false, Confirmed = true }
                }
            };

            Assert.Throws<InvalidOperationException>(() => roundEntity.GetWinnerIds());
        }
        
        [Test]
        public void GetWinnerIds_ThrowsExceptionWhenAtLeastOneMatchIsNotConfirmed()
        {
            var roundEntity = new RoundEntity()
            {
                Finished = true,
                Matches = new List<MatchEntity>()
                {
                    new MatchEntity() { Played = true, Confirmed = true },
                    new MatchEntity() { Played = true, Confirmed = false }
                }
            };

            Assert.Throws<InvalidOperationException>(() => roundEntity.GetWinnerIds());
        }
        
        [Test]
        public void GetWinnerIds_ThrowsExceptionWhenAtLeastOneMatchIsNotPlayedAndConfirmed()
        {
            var roundEntity = new RoundEntity()
            {
                Finished = true,
                Matches = new List<MatchEntity>()
                {
                    new MatchEntity() { Played = true, Confirmed = true },
                    new MatchEntity() { Played = false, Confirmed = false }
                }
            };

            Assert.Throws<InvalidOperationException>(() => roundEntity.GetWinnerIds());
        }

        [Test]
        public void GetWinnerIds_ThrowsExceptionWhenAtLeastOneMatchHasNullResult()
        {
            var roundEntity = new RoundEntity()
            {
                Finished = true,
                Matches = new List<MatchEntity>()
                {
                    new MatchEntity() { Result = null, Played = true, Confirmed = true }
                }
            };

            Assert.Throws<InvalidOperationException>(() => roundEntity.GetWinnerIds());
        }
    }
}