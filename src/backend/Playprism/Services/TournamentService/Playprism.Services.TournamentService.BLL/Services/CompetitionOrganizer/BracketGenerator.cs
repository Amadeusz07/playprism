using Playprism.Services.TournamentService.BLL.Common;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Services.CompetitionOrganizer
{
    public class BracketGenerator: IBracketGenerator
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IRepository<MatchDefinitionEntity> _matchDefinitionRepository;

        public BracketGenerator(IRoundRepository roundRepository, IMatchRepository matchRepository, IRepository<MatchDefinitionEntity> matchDefinitionRepository)
        {
            _roundRepository = roundRepository;
            _matchRepository = matchRepository;
            _matchDefinitionRepository = matchDefinitionRepository;
        }

        public async Task ClearBracketAsync(int tournamentId)
        {
            await _matchRepository.ClearAsync(tournamentId);
            await _roundRepository.ClearAsync(tournamentId);
        }

        public async Task<IEnumerable<RoundEntity>> GenerateRoundsAsync(TournamentEntity tournament, int participantsCount)
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
                FindAndSetNumberOfCompetitorsAndMathes(newRound, numberOfCompetitors, participantsCount);
                rounds.Add(newRound);
                await _roundRepository.AddAsync(newRound);
            }

            return rounds;
        }

        private void FindAndSetNumberOfCompetitorsAndMathes(RoundEntity newRound, int numberOfCompetitors, int participantsCount)
        {
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
                FindAndSetNumberOfCompetitorsAndMathes(newRound, newRound.NumberOfCompetitors, participantsCount);
            }
        }

        public async Task<IEnumerable<MatchEntity>> GenerateMatchesAsync(IEnumerable<RoundEntity> rounds)
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
                    await _matchRepository.AddAsync(newMatch);
                }
            }

            var newMatches = rounds.SelectMany(x => x.Matches);
            return newMatches;
        }
    }
}
