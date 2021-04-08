using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.DAL.Interfaces
{
    public interface IParticipantRepository: IRepository<ParticipantEntity>
    {
        Task<IEnumerable<ParticipantEntity>> GetParticipantsByUserIdAsync(string userId);
        Task<IEnumerable<ParticipantEntity>> GetByIdsAsync(IEnumerable<int> ids);

    }
}