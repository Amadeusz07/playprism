using Playprism.Services.TournamentService.DAL.Entities;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface ICrudService
    {
        Task<TournamentEntity> GetTournamentAsync(int id);
        Task<TournamentEntity> AddTournamentAsync(TournamentEntity entity);
        Task<TournamentEntity> UpdateTournamentAsync(TournamentEntity entity);
        Task DeleteTournamentAsync(int id);
        Task<ParticipantEntity> AddNewParticipantAsync(ParticipantEntity entity);
    }
}