using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

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

        public async Task<RoundEntity> FinishRoundAsync(int roundId)
        {
            var entity = MainDbContext.Rounds
                .Include(x => x.Matches)
                .First(x => x.Id == roundId);
            
            var toAutoResult = entity.Matches
                .Where(x => x.Participant1Id == null || x.Participant2Id == null);
            foreach (var matchToAutoResult in toAutoResult)
            {
                matchToAutoResult.AutoResult();
                MainDbContext.Entry(matchToAutoResult).State = EntityState.Modified;
                await MainDbContext.SaveChangesAsync();
            }
            
            entity.Finished = true;
            entity.EndDate = DateTime.UtcNow;
            MainDbContext.Entry(entity).State = EntityState.Modified;
            await MainDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<RoundEntity> GetNextRoundAsync(RoundEntity currentRound)
        {
            return await MainDbContext.Rounds
                .Include(x => x.Matches)
                .FirstOrDefaultAsync(x => x.Order == currentRound.Order + 1);
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