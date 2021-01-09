using System.Collections.Generic;
using System.Threading.Tasks;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.DAL.Interfaces
{
    public interface IRoundRepository: IRepository<RoundEntity>
    {
        Task<IEnumerable<RoundEntity>> AddAsync(IEnumerable<RoundEntity> entities);
        Task<RoundEntity> FinishRoundAsync(int roundId);
        Task<RoundEntity> GetNextRoundAsync(RoundEntity currentRound);
        Task<RoundEntity> GetPreviousRoundAsync(RoundEntity currentRound);
        Task ClearAsync(int tournamentId);
    }
}