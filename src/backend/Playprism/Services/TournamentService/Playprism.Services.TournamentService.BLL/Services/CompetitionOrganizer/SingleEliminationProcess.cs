using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Playprism.Services.TournamentService.BLL.Common;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.BLL.Services.CompetitionOrganizer
{
    public class SingleEliminationProcess: ICompetitionProcess
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IRepository<MatchDefinitionEntity> _matchDefinitionRepository;

        public SingleEliminationProcess(IParticipantRepository participantRepository, 
            ITournamentRepository tournamentRepository, 
            IRoundRepository roundRepository, 
            IMatchRepository matchRepository, 
            IRepository<MatchDefinitionEntity> matchDefinitionRepository)
        {
            _participantRepository = participantRepository;
            _tournamentRepository = tournamentRepository;
            _roundRepository = roundRepository;
            _matchRepository = matchRepository;
            _matchDefinitionRepository = matchDefinitionRepository;
        }

        public async Task GenerateBracketAsync(TournamentEntity tournament)
        {
            if (tournament.MaxNumberOfPlayers <= 1)
            {
                throw new ArgumentException("Number of players in tournament should be >= 2");
            }
            await _matchRepository.ClearAsync(tournament.Id);
            await _roundRepository.ClearAsync(tournament.Id);
            var participants = await _participantRepository.GetAsync(x => x.TournamentId == tournament.Id);
            if (participants != null)
            {
                var rounds = await GenerateRoundsAsync(tournament);
                var matches = await GenerateMatchesAsync(rounds, participants.Count());
                await ShuffleAsync(
                    matches.GroupBy(x => x.RoundId)
                        .OrderBy(x => x.Key)
                        .First(x => x.Any()), 
                    participants.Select(x => x.Id));
            }
        }

        private async Task<IEnumerable<RoundEntity>> GenerateRoundsAsync(TournamentEntity tournament)
        {
            const int numberOfMatchesInLastRound = 1;
            var defaultMatchDefinition = (await _matchDefinitionRepository
                .GetAsync(x => x.TournamentId == tournament.Id
                               && x.Name == Consts.DefaultSettingsName)).First();
            var rounds = new List<RoundEntity>();
            for (var i = 0; i < tournament.MaxNumberOfPlayers; i++)
            {
                var previousRound = rounds.LastOrDefault();
                if (previousRound != null && previousRound?.NumberOfCompetitors / 2 == numberOfMatchesInLastRound)
                {
                    break;
                }
                var newRound = new RoundEntity()
                {
                    TournamentId = tournament.Id,
                    MatchDefinitionId = defaultMatchDefinition.Id,
                    Order = i,
                    NumberOfCompetitors = previousRound?.NumberOfCompetitors / 2 ?? tournament.MaxNumberOfPlayers,
                    NumberOfMatches = previousRound?.NumberOfMatches / 2 ?? tournament.MaxNumberOfPlayers / 2,
                    StartDate = null,
                    EndDate = null,
                    Finished = false,
                    Started = false,
                    Matches = new List<MatchEntity>()
                };
                
                rounds.Add(newRound);
                await _roundRepository.AddAsync(newRound);
            }
            
            return rounds;
        }

        private async Task<IEnumerable<MatchEntity>> GenerateMatchesAsync(IEnumerable<RoundEntity> rounds, int participantsCount)
        {
            foreach (var round in rounds)
            {
                if (round.NumberOfCompetitors / 2 < participantsCount)
                {
                    for (var i = 0; i < round.NumberOfMatches; i++)
                    {
                        var newMatch = new MatchEntity()
                        {
                            TournamentId = round.TournamentId,
                            RoundId = round.Id,
                            MatchDate = null,
                            Participant1Id = null,
                            Participant2Id = null,
                            PreviousMatch1Id = null,
                            PreviousMatch2Id = null, 
                            Result = null,
                            Played = false,
                            Confirmed = !round.MatchDefinition.ConfirmationNeeded
                        };
                        round.Matches.Add(newMatch);
                    }
                }
            }

            var newMatches = rounds.SelectMany(x => x.Matches);
            await _matchRepository.AddRangeAsync(newMatches);
            
            return newMatches;
        }

        private async Task<IEnumerable<MatchEntity>> ShuffleAsync(IEnumerable<MatchEntity> matches, IEnumerable<int> participantIds)
        {
            var random = new Random();
            var pool = participantIds.ToList();
            foreach (var match in matches)
            {
                if (!pool.Any())
                {
                    match.Participant1Id = null;
                    match.Participant2Id = null;
                    await _matchRepository.UpdateAsync(match);
                    break;
                }
                var randomParticipant = pool[random.Next(pool.Count())];
                match.Participant1Id = randomParticipant;
                pool.Remove(randomParticipant);
                if (!pool.Any())
                {
                    match.Participant2Id = null;
                    await _matchRepository.UpdateAsync(match);
                    break;
                }
                randomParticipant = pool[random.Next(pool.Count())];
                match.Participant2Id = randomParticipant;
                pool.Remove(randomParticipant);

                await _matchRepository.UpdateAsync(match);
            }

            return matches;
        }

        public async Task FinishRoundAsync(int roundId)
        {
            var round = await _roundRepository.FinishRoundAsync(roundId);
            var nextRound = await _roundRepository.GetNextRoundAsync(round);
            if (nextRound == null)
            {
                var tournament = await _tournamentRepository.GetByIdAsync(round.TournamentId);
                tournament.Finished = true;
                tournament.EndDate = DateTime.UtcNow;
                await _tournamentRepository.UpdateAsync(tournament);
            }
            else
            {
                var winnersFromPreviousRound = round.GetWinnerIds();
                await ShuffleAsync(nextRound.Matches, winnersFromPreviousRound);
            }
        }
    }
}