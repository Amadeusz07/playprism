using Playprism.Services.TournamentService.BLL.Common;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IBracketGenerator _bracketGenerator;
        private readonly IShuffler _shuffler;

        public SingleEliminationProcess(
            IParticipantRepository participantRepository, 
            ITournamentRepository tournamentRepository, 
            IRoundRepository roundRepository, 
            IMatchRepository matchRepository, 
            IBracketGenerator bracketGenerator, 
            IShuffler shuffler)
        {
            _participantRepository = participantRepository;
            _tournamentRepository = tournamentRepository;
            _roundRepository = roundRepository;
            _matchRepository = matchRepository;
            _bracketGenerator = bracketGenerator;
            _shuffler = shuffler;
        }

        public async Task StartTournamentAsync(TournamentEntity tournament)
        {
            //tournament = (await _tournamentRepository.GetAsync(x => x.Id == tournament.Id, includeString: "Participants", disableTracking: true)).Single();
            //var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            //if (tournament.Participants.Count < 2)
            //{
            //    throw new ValidationException("Tournament has to contain at least 2 participants");
            //}
            tournament.Ongoing = true;
            await _tournamentRepository.UpdateAsync(tournament);
            await GenerateBracketAsync(tournament);
        }

        public async Task GenerateBracketAsync(TournamentEntity tournament)
        {
            await _bracketGenerator.ClearBracketAsync(tournament.Id);
            var participants = await _participantRepository.GetAsync(x => x.TournamentId == tournament.Id);
            if (participants == null)
            {
                throw new InvalidOperationException("No participtans found for given tournament");
            }

            var rounds = await _bracketGenerator.GenerateRoundsAsync(tournament, participants.Count());
            var matches = await _bracketGenerator.GenerateMatchesAsync(rounds);
            await _shuffler.ShuffleAsync(
                matches.GroupBy(x => x.RoundId)
                    .OrderBy(x => x.Key)
                    .First(x => x.Any()),
                participants.Select(x => (int?)x.Id));

            var firstRound = rounds.OrderBy(x => x.Order).First();
            firstRound.StartDate = DateTime.UtcNow;
            firstRound.Started = true;
            await _roundRepository.UpdateAsync(firstRound);
        }

        public async Task CloseRoundAsync(int tournamentId)
        {
            var finishedRound = await FinishRound(tournamentId);
            var nextRound = await _roundRepository.GetNextRoundAsync(finishedRound);
            if (nextRound == null)
            {
                await FinishTournament(finishedRound);
            }
            else
            {
                var winnersFromPreviousRound = finishedRound.GetWinnersByMatchId();
                await SetupNextRoundMatches(nextRound, winnersFromPreviousRound);
                nextRound.Started = true;
                nextRound.StartDate = DateTime.UtcNow;
                await _roundRepository.UpdateAsync(nextRound);
            }
        }

        private async Task FinishTournament(RoundEntity finishedRound)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(finishedRound.TournamentId);
            tournament.Finished = true;
            tournament.EndDate = DateTime.UtcNow;
            await _tournamentRepository.UpdateAsync(tournament);
        }

        private async Task<RoundEntity> FinishRound(int tournamentId)
        {
            var roundToFinish = await _roundRepository.GetRoundToFinishAsync(tournamentId);
            if (roundToFinish == null)
            {
                throw new ValidationException("No rounds available to close");
            }

            var toAutoResult = roundToFinish.Matches
                .Where(x => x.Participant1Id == null || x.Participant2Id == null);
            foreach (var matchToAutoResult in toAutoResult)
            {
                matchToAutoResult.AutoResult();
                await _matchRepository.UpdateAsync(matchToAutoResult);
            }

            roundToFinish.Finished = true;
            roundToFinish.EndDate = DateTime.UtcNow;
            await _roundRepository.UpdateAsync(roundToFinish);

            return roundToFinish;
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
            var participants = await _participantRepository.GetAsync(x => x.TournamentId == tournamentId);
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

            BracketResponse bracketResponse = GenerateEmptyResponseRounds(rounds);

            for (int i = 0; i < rounds.Count; i++)
            {
                if (i == 0)
                {
                    var finalMatch = rounds.First().Matches.First();
                    var matchResponse = GenerateMatchResponse(
                        finalMatch,
                        participants.GetName(finalMatch.Participant1Id),
                        participants.GetName(finalMatch.Participant2Id)
                    );
                    bracketResponse.Rounds.First().Matches.Add(matchResponse); 
                }
                foreach (var match in bracketResponse.Rounds[i].Matches)
                {
                    var isLastRound = i + 1 <= bracketResponse.Rounds.Count - 1;
                    if (isLastRound)
                    {
                        var previousRoundResponse = bracketResponse.Rounds.ElementAt(i + 1);
                        var previousRoundMatches = rounds[i + 1].Matches;

                        var previousMatch1 = previousRoundMatches.Single(x => x.Id == match.PreviousMatch1Id);
                        var matchResponse1 = GenerateMatchResponse(
                            previousMatch1, 
                            participants.GetName(previousMatch1.Participant1Id),
                            participants.GetName(previousMatch1.Participant2Id)
                        );
                        previousRoundResponse.Matches.Add(matchResponse1);


                        var previousMatch2 = previousRoundMatches.Single(x => x.Id == match.PreviousMatch2Id);
                        var matchResponse2 = GenerateMatchResponse(
                            previousMatch2,
                            participants.GetName(previousMatch2.Participant1Id),
                            participants.GetName(previousMatch2.Participant2Id)
                        );
                        previousRoundResponse.Matches.Add(matchResponse2);

                    }
                }
            }

            bracketResponse.Rounds.Reverse();
            return bracketResponse;
        }

        private BracketResponse GenerateEmptyResponseRounds(IReadOnlyList<RoundEntity> rounds)
        {
            var bracket = new BracketResponse();
            foreach (var round in rounds)
            {
                bracket.Rounds.Add(new RoundResponse()
                {
                    Id = round.Id,
                    RoundDate = round.StartDate
                });
            }

            return bracket;
        }

        private MatchResponse GenerateMatchResponse(MatchEntity match, string participant1Name, string participant2Name)
        {
            return new MatchResponse()
            {
                Id = match.Id,
                MatchDate = match.MatchDate,
                PreviousMatch1Id = match.PreviousMatch1Id,
                PreviousMatch2Id = match.PreviousMatch2Id,
                Participant1 = participant1Name,
                Participant2 = participant2Name,
                Participant1Score = match.Participant1Score,
                Participant2Score = match.Participant2Score,
                Result = match.Result
            };
        }
    }
}