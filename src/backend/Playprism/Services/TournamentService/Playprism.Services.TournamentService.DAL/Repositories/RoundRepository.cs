using Microsoft.EntityFrameworkCore;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.DAL.Repositories
{
    internal class RoundRepository: Repository<RoundEntity>, IRoundRepository
    {
        public RoundRepository(TournamentDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public async Task<IEnumerable<RoundEntity>> AddAsync(IEnumerable<RoundEntity> entities)
        {
            await MainDbContext.Rounds.AddRangeAsync(entities);
            await MainDbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<RoundEntity> GetRoundToFinishAsync(int tournamentId)
        {
            return await MainDbContext.Rounds
                .Include(x => x.Matches)
                .Where(x => x.TournamentId == tournamentId && !x.Finished && x.Started)
                .OrderBy(x => x.Order)
                .FirstOrDefaultAsync();
        }

        public async Task<RoundEntity> GetNextRoundAsync(RoundEntity currentRound)
        {
            return await MainDbContext.Rounds
                .Include(x => x.Matches)
                .FirstOrDefaultAsync(x => x.TournamentId == currentRound.TournamentId && x.Order == currentRound.Order + 1);
        }
        
        public async Task<RoundEntity> GetPreviousRoundAsync(RoundEntity currentRound)
        {
            return await MainDbContext.Rounds
                .Include(x => x.Matches)
                .FirstOrDefaultAsync(x => x.Order == currentRound.Order - 1);
        }

        public async Task ClearAsync(int tournamentId)
        {
            var toRemove = MainDbContext.Rounds
                .Where(x => x.TournamentId == tournamentId);
            MainDbContext.Rounds.RemoveRange(toRemove);
            await MainDbContext.SaveChangesAsync();
        }
    }
}