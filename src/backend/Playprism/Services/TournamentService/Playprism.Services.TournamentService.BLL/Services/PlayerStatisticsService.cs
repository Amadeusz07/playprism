using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Services
{
    public class PlayerStatisticsService: IPlayerStatisticsService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IRepository<DisciplineEntity> _disciplineRepository;

        public PlayerStatisticsService(IParticipantRepository participantRepository, IMatchRepository matchRepository, IRepository<DisciplineEntity> disciplineRepository)
        {
            _participantRepository = participantRepository;
            _matchRepository = matchRepository;
            _disciplineRepository = disciplineRepository;
        }

        public async Task<IEnumerable<PlayerRecordsResponse>> GetPlayerRecords(string userId)
        {
            var result = new List<PlayerRecordsResponse>();
            var participantData = await _participantRepository.GetAsync(
                x => x.UserId == userId && x.Approved && !x.Tournament.AreTeams, 
                includeString: "Tournament"
            );
            if (participantData == null || participantData.Count == 0)
            {
                return result;
            }
            var groupedByDisciplines = participantData
                .GroupBy(x => x.Tournament.DisciplineId)
                .Select(x => new { DisciplineId = x.Key, PartiticipantData = x } );
            foreach (var discipline in groupedByDisciplines)
            {
                var currentDiscipline = await _disciplineRepository.GetByIdAsync(discipline.DisciplineId);
                var newRecords = await CreatePlayerRecords(discipline.PartiticipantData, currentDiscipline);
                result.Add(newRecords);
            }

            return result;
        }

        private async Task<PlayerRecordsResponse> CreatePlayerRecords(IEnumerable<ParticipantEntity> participantData, DisciplineEntity currentDiscipline)
        {
            const int participant1AsWinner = 1;
            const int participant2AsWinner = 2;

            var participantIds = participantData.Select(x => x.Id).ToList();

            var matchesAsParticipant1 = await _matchRepository.GetCompletedMatchesByParticipant1Ids(participantIds);
            var matchesAsParticipant2 = await _matchRepository.GetCompletedMatchesByParticipant2Ids(participantIds);

            var numberOfWins = matchesAsParticipant1.Where(x => x.Result == participant1AsWinner).Count() +
                matchesAsParticipant2.Where(x => x.Result == participant2AsWinner).Count();
            var numberOfDefeats = matchesAsParticipant1.Where(x => x.Result == participant2AsWinner).Count() +
                matchesAsParticipant2.Where(x => x.Result == participant1AsWinner).Count();

            var newRecords = new PlayerRecordsResponse
            {
                Name = currentDiscipline.Name,
                Series = new List<WinsDefeatsResponse>()
                {
                    new WinsDefeatsResponse { Name = "Wins", Value = numberOfWins },
                    new WinsDefeatsResponse { Name = "Defeats", Value = numberOfDefeats }
                }
            };

            return newRecords;
        }
    }
}
