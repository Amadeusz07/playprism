using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Services.CompetitionOrganizer
{
    internal class Shuffler: IShuffler
    {
        private readonly IMatchRepository _matchRepository;

        public Shuffler(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<MatchEntity>> ShuffleAsync(IEnumerable<MatchEntity> matches, IEnumerable<int?> participantIds)
        {
            var totalNumberOfPlayers = matches.Count() * 2;
            var pool = participantIds.ToList();
            var numberOfEmptyParticipants = totalNumberOfPlayers - pool.Count;
            for (int i = 0; i < numberOfEmptyParticipants; i++)
            {
                pool.Add(null);
            }

            var random = new Random();
            foreach (var match in matches)
            {
                var randomParticipant = pool[random.Next(pool.Count)];
                match.Participant1Id = randomParticipant;
                pool.Remove(randomParticipant);

                randomParticipant = pool[random.Next(pool.Count)];
                match.Participant2Id = randomParticipant;
                pool.Remove(randomParticipant);

                await _matchRepository.UpdateAsync(match);
            }

            return matches;
        }
    }
}
