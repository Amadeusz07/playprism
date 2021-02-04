using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentEntity>> GetTournamentsByDiscipline(int disciplineId);
        Task<TournamentEntity> GetTournamentAsync(int id);
        Task<TournamentEntity> AddTournamentAsync(TournamentEntity entity);
        Task<TournamentEntity> UpdateTournamentAsync(TournamentEntity entity);
        Task DeleteTournamentAsync(int id);
        Task<ParticipantEntity> AddNewParticipantAsync(ParticipantEntity entity);
    }
}