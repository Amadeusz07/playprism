using Playprism.Services.TournamentService.BLL.Common;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                var rounds = await GenerateRoundsAsync(tournament, participants.Count());
                var matches = await GenerateMatchesAsync(rounds);
                await ShuffleAsync(
                    matches.GroupBy(x => x.RoundId)
                        .OrderBy(x => x.Key)
                        .First(x => x.Any()), 
                    participants.Select(x => x.Id));
            }
        }

        private async Task<IEnumerable<RoundEntity>> GenerateRoundsAsync(TournamentEntity tournament, int participantsCount)
        {
            const int numberOfMatchesInLastRound = 1;
            var defaultMatchDefinition = (await _matchDefinitionRepository
                .GetAsync(x => x.TournamentId == tournament.Id
                               && x.Name == Consts.DefaultSettingsName)).First();
            var rounds = new List<RoundEntity>();
            for (var i = 0; i < tournament.MaxNumberOfPlayers; i++)
            {
                var previousRound = rounds.LastOrDefault();
                var lastRoundCreated = previousRound != null &&
                                       previousRound?.NumberOfCompetitors / 2 == numberOfMatchesInLastRound;
                if (lastRoundCreated)
                {
                    break;
                }
                
                var newRound = new RoundEntity()
                {
                    TournamentId = tournament.Id,
                    MatchDefinitionId = defaultMatchDefinition.Id,
                    Order = i,
                    StartDate = null,
                    EndDate = null,
                    Finished = false,
                    Started = false,
                    Matches = new List<MatchEntity>()
                };
                var numberOfCompetitors = previousRound?.NumberOfCompetitors / 2 ?? tournament.MaxNumberOfPlayers;
                var enoughCompetitors = numberOfCompetitors / 2 < participantsCount;
                if (enoughCompetitors)
                {
                    newRound.NumberOfCompetitors = numberOfCompetitors;
                    newRound.NumberOfMatches = numberOfCompetitors / 2;
                }
                else
                {
                    var numberOfCompetitorsInNextRound = numberOfCompetitors / 2;
                    newRound.NumberOfCompetitors = numberOfCompetitorsInNextRound;
                    newRound.NumberOfMatches = numberOfCompetitorsInNextRound / 2;
                }
                rounds.Add(newRound);
                await _roundRepository.AddAsync(newRound);
            }
            
            return rounds;
        }
        
        private async Task<IEnumerable<MatchEntity>> GenerateMatchesAsync(IEnumerable<RoundEntity> rounds)
        {
            for (var i = 0; i < rounds.Count(); i++)
            {
                var round = rounds.ElementAt(i);
                IEnumerator<MatchEntity> previousMatches = null;
                if (i != 0)
                {
                    var previousRound = rounds.ElementAt(i - 1);
                    previousMatches = previousRound.Matches.GetEnumerator();
                }
                for (var j = 0; j < round.NumberOfMatches; j++)
                {
                    var newMatch = new MatchEntity()
                    {
                        TournamentId = round.TournamentId,
                        RoundId = round.Id,
                        MatchDate = null,
                        Participant1Id = null,
                        Participant2Id = null,
                        Result = null,
                        Played = false,
                        Confirmed = !round.MatchDefinition.ConfirmationNeeded
                    };
                    if (previousMatches != null)
                    {
                        previousMatches.MoveNext();
                        newMatch.PreviousMatch1Id = previousMatches.Current.Id;
                        previousMatches.MoveNext();
                        newMatch.PreviousMatch2Id = previousMatches.Current.Id;
                    }
                    round.Matches.Add(newMatch);
                    await _matchRepository.AddAsync(newMatch);
                }
            }
            
            var newMatches = rounds.SelectMany(x => x.Matches);
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
            var finishedRound = await _roundRepository.FinishRoundAsync(roundId);
            var nextRound = await _roundRepository.GetNextRoundAsync(finishedRound);
            if (nextRound == null)
            {
                var tournament = await _tournamentRepository.GetByIdAsync(finishedRound.TournamentId);
                tournament.Finished = true;
                tournament.EndDate = DateTime.UtcNow;
                await _tournamentRepository.UpdateAsync(tournament);
            }
            else
            {
                var winnersFromPreviousRound = finishedRound.GetWinnersByMatchId();
                await SetupNextRoundMatches(nextRound, winnersFromPreviousRound);
            }
        }

        private async Task SetupNextRoundMatches(RoundEntity nextRound, Dictionary<int, int?> winnerIdsByMatchId)
        {
            foreach (var match in nextRound.Matches)
            {
                match.Participant1Id = winnerIdsByMatchId[match.PreviousMatch1Id.Value];
                match.Participant2Id = winnerIdsByMatchId[match.PreviousMatch2Id.Value];

                await _matchRepository.UpdateAsync(match);
            }
        }
        
        public async Task<BracketResponse> GenerateResponseBracketAsync(int tournamentId)
        {
            var bracket = new BracketResponse();
            var rounds = await _roundRepository.GetAsync(
                x => x.TournamentId == tournamentId, 
                x => x.OrderByDescending(y => y.Order), 
                "Matches",
                true
            );
            if (rounds == null)
            {
                throw new EntityNotFoundException();
            }
            var participants = await _participantRepository.GetAsync(x => x.TournamentId == tournamentId);
            
            foreach (var round in rounds)
            {
                bracket.Rounds.Add(new RoundResponse()
                {
                    Id = round.Id,
                    RoundDate = round.StartDate
                });
            }

            for (int i = 0; i < rounds.Count; i++)
            {
                foreach (var match in rounds[i].Matches)
                {
                    var currentRoundBracket = bracket.Rounds.ElementAt(i);
                    if (!currentRoundBracket.Matches.Any())
                    {
                        currentRoundBracket.Matches.Add(new MatchResponse()
                        {
                            Id = match.Id,
                            MatchDate = match.MatchDate,
                            Participant1 = participants.SingleOrDefault(x => x.Id == match.Participant1Id)?.Name,
                            Participant2 = participants.SingleOrDefault(x => x.Id == match.Participant2Id)?.Name,
                            Participant1Score = match.Participant1Score,
                            Participant2Score = match.Participant2Score,
                            Result = match.Result
                        });
                    }

                    if (i + 1 <= bracket.Rounds.Count - 1)
                    {
                        var previousRoundBracket = bracket.Rounds.ElementAt(i + 1);

                        if (match.PreviousMatch1Id != null)
                        {
                            var previousMatch1 = rounds[i + 1].Matches.Single(x => x.Id == match.PreviousMatch1Id);
                            previousRoundBracket.Matches.Add(new MatchResponse()
                            {
                                Id = previousMatch1.Id,
                                MatchDate = previousMatch1.MatchDate,
                                Participant1 = participants.SingleOrDefault(x => x.Id == previousMatch1.Participant1Id)
                                    ?.Name,
                                Participant2 = participants.SingleOrDefault(x => x.Id == previousMatch1.Participant2Id)
                                    ?.Name,
                                Participant1Score = previousMatch1.Participant1Score,
                                Participant2Score = previousMatch1.Participant2Score,
                                Result = previousMatch1.Result
                            });
                        }

                        if (match.PreviousMatch2Id != null)
                        {
                            var previousMatch2 = rounds[i + 1].Matches.Single(x => x.Id == match.PreviousMatch2Id);
                            previousRoundBracket.Matches.Add(new MatchResponse()
                            {
                                Id = previousMatch2.Id,
                                MatchDate = previousMatch2.MatchDate,
                                Participant1 = participants.SingleOrDefault(x => x.Id == previousMatch2.Participant1Id)
                                    ?.Name,
                                Participant2 = participants.SingleOrDefault(x => x.Id == previousMatch2.Participant2Id)
                                    ?.Name,
                                Participant1Score = previousMatch2.Participant1Score,
                                Participant2Score = previousMatch2.Participant2Score,
                                Result = previousMatch2.Result
                            });
                        }
                    }
                }
            }

            bracket.Rounds.Reverse();
            return bracket;
        }


    }
}