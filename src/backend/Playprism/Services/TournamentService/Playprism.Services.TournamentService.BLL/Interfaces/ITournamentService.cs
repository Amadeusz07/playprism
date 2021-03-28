using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentListItemResponse>> GetTournamentsByDisciplineAsync(int disciplineId);
        Task<TournamentEntity> GetTournamentAsync(int id);
        Task<TournamentDetailsResponse> GetTournamentDetailsAsync(int id);
        Task<TournamentEntity> AddTournamentAsync(CreateTournamentRequest entity);
        Task<TournamentEntity> UpdateTournamentAsync(int id, UpdateTournamentRequest request);
        Task DeleteTournamentAsync(int id);
        Task<ParticipantEntity> AddNewParticipantAsync(ParticipantEntity entity);
        Task<CanJoinResponse> CanJoinTournament(JoinTournamentRequest joinRequest, int tournamentId);
    }
}